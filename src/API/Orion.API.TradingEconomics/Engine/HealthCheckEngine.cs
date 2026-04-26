using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;
using HealthReport = Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Monitors registered trading-system components and reports system health.
    /// </summary>
    public sealed class HealthCheckEngine : IHealthCheckEngine, IAsyncDisposable
    {
        private readonly ILogger<HealthCheckEngine> _logger;
        private readonly HealthCheckOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, HealthComponent> _components = new();
        private readonly ConcurrentQueue<HealthSnapshot> _history = new();
        private readonly SemaphoreSlim _checkLock = new(1, 1);
        private readonly Timer _monitoringTimer;

        private HealthStatus _currentStatus = HealthStatus.Healthy;

        public HealthCheckEngine(
            ILogger<HealthCheckEngine> logger,
            IOptions<HealthCheckOptions> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new HealthCheckOptions();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            RegisterDefaultComponents();

            _monitoringTimer = new Timer(
                async _ => await RunHealthChecksAsync(),
                null,
                TimeSpan.FromSeconds(_options.InitialDelaySeconds),
                TimeSpan.FromSeconds(_options.CheckIntervalSeconds));
        }

        /// <inheritdoc />
        public void RegisterComponent(HealthComponent component)
        {
            ArgumentNullException.ThrowIfNull(component);

            if (string.IsNullOrWhiteSpace(component.Name))
                throw new ArgumentException("Component name is required.", nameof(component));

            component.LastCheckTime = DateTime.UtcNow;
            _components[component.Name] = component;

            _logger.LogInformation("Registered health component: {Component}", component.Name);
        }

        /// <inheritdoc />
        public async Task<HealthReport> RunHealthChecksAsync(CancellationToken cancellationToken = default)
        {
            if (!await _checkLock.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
            {
                _logger.LogWarning("Health check already in progress, returning cached report");
                return CreateCachedReport();
            }

            try
            {
                var tasks = _components.Values
                    .Where(ShouldCheck)
                    .Select(component => CheckComponentAsync(component, cancellationToken));

                var results = await Task.WhenAll(tasks);

                var entries = new Dictionary<string, HealthReportEntry>();

                foreach (var (component, result) in results)
                {
                    component.LastCheckTime = DateTime.UtcNow;
                    component.LastResult = result;
                    component.TotalChecks++;

                    if (result.Status == HealthStatus.Unhealthy)
                        component.FailedChecks++;

                    entries[component.Name] = new HealthReportEntry(
                        result.Status,
                        result.Description,
                        result.Duration,
                        result.Exception,
                        result.Data,
                        result.Tags);
                }

                var status = DetermineOverallStatus(results.ToDictionary(x => x.Component.Name, x => x.Result));
                LogStatusChange(status, entries);

                _currentStatus = status;

                AddSnapshot(status, entries);

                return new HealthReport(entries, status, TimeSpan.Zero);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check execution failed");
                return CreateErrorReport(ex);
            }
            finally
            {
                _checkLock.Release();
            }
        }

        /// <inheritdoc />
        public Task<HealthReport> GetCurrentHealthAsync()
        {
            return Task.FromResult(CreateCachedReport());
        }

        /// <inheritdoc />
        public Task<HealthTrend> GetHealthTrendAsync(int hours = 24)
        {
            var snapshots = _history
                .Where(x => x.Timestamp >= DateTime.UtcNow.AddHours(-hours))
                .OrderBy(x => x.Timestamp)
                .ToList();

            return Task.FromResult(new HealthTrend
            {
                Snapshots = snapshots,
                UptimePercentage = CalculateUptime(snapshots),
                MeanTimeToRecovery = CalculateMttr(snapshots),
                StatusDistribution = snapshots
                    .GroupBy(x => x.Status)
                    .ToDictionary(x => x.Key, x => x.Count())
            });
        }

        /// <inheritdoc />
        public Task<ComponentDetails?> GetComponentDetailsAsync(string componentName)
        {
            if (string.IsNullOrWhiteSpace(componentName))
                throw new ArgumentException("Component name is required.", nameof(componentName));

            if (!_components.TryGetValue(componentName, out var component))
                return Task.FromResult<ComponentDetails?>(null);

            return Task.FromResult<ComponentDetails?>(new ComponentDetails
            {
                Name = component.Name,
                Type = component.Type,
                Critical = component.Critical,
                LastCheck = component.LastCheckTime,
                LastResult = component.LastResult,
                TotalChecks = component.TotalChecks,
                FailedChecks = component.FailedChecks,
                SuccessRate = component.TotalChecks > 0
                    ? (double)(component.TotalChecks - component.FailedChecks) / component.TotalChecks * 100
                    : 0
            });
        }

        /// <inheritdoc />
        public void EnableComponent(string name, bool enabled)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Component name is required.", nameof(name));

            if (_components.TryGetValue(name, out var component))
            {
                component.Enabled = enabled;
                _logger.LogInformation("Component {Component} enabled: {Enabled}", name, enabled);
            }
        }

        /// <inheritdoc />
        public async Task GracefulShutdownAsync()
        {
            _logger.LogInformation("Shutting down health monitoring");

            await _monitoringTimer.DisposeAsync();
            await RunHealthChecksAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _monitoringTimer.DisposeAsync();
            _checkLock.Dispose();
        }

        private void RegisterDefaultComponents()
        {
            RegisterComponent(new HealthComponent
            {
                Name = "NormalizationEngine",
                Type = HealthComponentType.PipelineEngine,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(5)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "RegimeEngine",
                Type = HealthComponentType.PipelineEngine,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(5)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "SignalEngine",
                Type = HealthComponentType.PipelineEngine,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(5)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "RiskEngine",
                Type = HealthComponentType.PipelineEngine,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(3)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "ExecutionEngine",
                Type = HealthComponentType.PipelineEngine,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(5)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "AuditTrailStorage",
                Type = HealthComponentType.Infrastructure,
                Critical = true,
                Timeout = TimeSpan.FromSeconds(5)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "MemoryUsage",
                Type = HealthComponentType.Infrastructure,
                Critical = true,
                CheckInterval = TimeSpan.FromMinutes(1)
            });

            RegisterComponent(new HealthComponent
            {
                Name = "CpuUsage",
                Type = HealthComponentType.Infrastructure,
                Critical = false,
                CheckInterval = TimeSpan.FromMinutes(1)
            });
        }

        private async Task<(HealthComponent Component, HealthCheckResult Result)> CheckComponentAsync(
            HealthComponent component,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeout.CancelAfter(component.Timeout);

                var result = component.Type switch
                {
                    HealthComponentType.PipelineEngine => await CheckPipelineEngineAsync(component, timeout.Token),
                    HealthComponentType.Infrastructure => await CheckInfrastructureAsync(component, timeout.Token),
                    _ => HealthCheckResult.Healthy($"{component.Name} operational")
                };

                stopwatch.Stop();

                var data = new Dictionary<string, object>(result.Data)
                {
                    ["ResponseTimeMs"] = stopwatch.ElapsedMilliseconds,
                    ["LastCheckUtc"] = DateTime.UtcNow
                };

                if (component.DegradedThreshold > TimeSpan.Zero &&
                    stopwatch.Elapsed > component.DegradedThreshold)
                {
                    return (component, HealthCheckResult.Degraded(
                        $"Response time {stopwatch.ElapsedMilliseconds}ms exceeds threshold.",
                        data: data));
                }

                return (component, new HealthCheckResult(
                    result.Status,
                    result.Description,
                    result.Exception,
                    data));
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();

                return component.Critical
                    ? (component, HealthCheckResult.Unhealthy($"Timeout after {stopwatch.ElapsedMilliseconds}ms"))
                    : (component, HealthCheckResult.Degraded($"Timeout after {stopwatch.ElapsedMilliseconds}ms"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed for {Component}", component.Name);

                return component.Critical
                    ? (component, HealthCheckResult.Unhealthy($"Check failed: {ex.Message}", ex))
                    : (component, HealthCheckResult.Degraded($"Check failed: {ex.Message}", ex));
            }
        }

        private Task<HealthCheckResult> CheckPipelineEngineAsync(
            HealthComponent component,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var engine = component.Name switch
            {
                "NormalizationEngine" => _serviceProvider.GetService(typeof(NormalizationEngine)),
                "RegimeEngine" => _serviceProvider.GetService(typeof(RegimeEngine)),
                "SignalEngine" => _serviceProvider.GetService(typeof(SignalEngine)),
                "RiskEngine" => _serviceProvider.GetService(typeof(RiskEngine)),
                "ExecutionEngine" => _serviceProvider.GetService(typeof(ExecutionEngine)),
                _ => null
            };

            if (engine == null)
                return Task.FromResult(HealthCheckResult.Unhealthy($"{component.Name} not registered in DI"));

            return Task.FromResult(HealthCheckResult.Healthy($"{component.Name} operational"));
        }

        private async Task<HealthCheckResult> CheckInfrastructureAsync(
            HealthComponent component,
            CancellationToken cancellationToken)
        {
            return component.Name switch
            {
                "MemoryUsage" => CheckMemoryUsage(),
                "CpuUsage" => CheckCpuUsage(),
                "AuditTrailStorage" => await CheckAuditStorageAsync(cancellationToken),
                _ => HealthCheckResult.Healthy($"{component.Name} operational")
            };
        }

        private HealthCheckResult CheckMemoryUsage()
        {
            var process = Process.GetCurrentProcess();
            var usedMemoryMb = process.WorkingSet64 / 1024.0 / 1024.0;

            var data = new Dictionary<string, object>
            {
                ["WorkingSetMB"] = Math.Round(usedMemoryMb, 2),
                ["ManagedMemoryMB"] = Math.Round(GC.GetTotalMemory(false) / 1024.0 / 1024.0, 2)
            };

            return usedMemoryMb > _options.MaxMemoryThresholdMB
                ? HealthCheckResult.Degraded($"High memory usage: {usedMemoryMb:F1}MB", data: data)
                : HealthCheckResult.Healthy($"Memory usage: {usedMemoryMb:F1}MB", data: data);
        }

        private static HealthCheckResult CheckCpuUsage()
        {
            var process = Process.GetCurrentProcess();

            return HealthCheckResult.Healthy(
                $"CPU time: {process.TotalProcessorTime.TotalSeconds:F1}s",
                data: new Dictionary<string, object>
                {
                    ["ThreadCount"] = process.Threads.Count
                });
        }

        private async Task<HealthCheckResult> CheckAuditStorageAsync(CancellationToken cancellationToken)
        {
            var storage = _serviceProvider.GetService(typeof(IAuditStorage)) as IAuditStorage;

            if (storage == null)
                return HealthCheckResult.Unhealthy("Audit storage not registered");

            var query = new AuditQuery
            {
                StartDate = DateTime.UtcNow.AddMinutes(-5),
                PageSize = 1
            };

            await storage.QueryAsync(query);

            return HealthCheckResult.Healthy("Audit storage accessible");
        }

        private HealthStatus DetermineOverallStatus(Dictionary<string, HealthCheckResult> checks)
        {
            var criticalUnhealthy = _components.Values
                .Where(x => x.Critical)
                .Any(x => checks.TryGetValue(x.Name, out var result) &&
                          result.Status == HealthStatus.Unhealthy);

            if (criticalUnhealthy)
                return HealthStatus.Unhealthy;

            if (checks.Any(x => x.Value.Status != HealthStatus.Healthy))
                return HealthStatus.Degraded;

            return HealthStatus.Healthy;
        }

        private bool ShouldCheck(HealthComponent component)
        {
            if (!component.Enabled)
                return false;

            if (component.CheckInterval.HasValue)
                return DateTime.UtcNow - component.LastCheckTime >= component.CheckInterval.Value;

            return true;
        }

        private HealthReport CreateCachedReport()
        {
            var entries = _components.Values
                .Where(x => x.LastResult != null)
                .ToDictionary(
                    x => x.Name,
                    x => new HealthReportEntry(
                        x.LastResult!.Status,
                        x.LastResult.Description,
                        x.LastResult.Duration,
                        x.LastResult.Exception,
                        x.LastResult.Data,
                        x.LastResult.Tags));

            return new HealthReport(entries, _currentStatus, TimeSpan.Zero);
        }

        private static HealthReport CreateErrorReport(Exception ex)
        {
            var entries = new Dictionary<string, HealthReportEntry>
            {
                ["HealthCheckEngine"] = new(
                    HealthStatus.Unhealthy,
                    $"Health check system failed: {ex.Message}",
                    TimeSpan.Zero,
                    ex,
                    new Dictionary<string, object>(),
                    [])
            };

            return new HealthReport(entries, HealthStatus.Unhealthy, TimeSpan.Zero);
        }

        private void AddSnapshot(
            HealthStatus status,
            IReadOnlyDictionary<string, HealthReportEntry> entries)
        {
            _history.Enqueue(new HealthSnapshot
            {
                Timestamp = DateTime.UtcNow,
                Status = status,
                ComponentCount = entries.Count,
                HealthyCount = entries.Count(x => x.Value.Status == HealthStatus.Healthy),
                DegradedCount = entries.Count(x => x.Value.Status == HealthStatus.Degraded),
                UnhealthyCount = entries.Count(x => x.Value.Status == HealthStatus.Unhealthy)
            });

            while (_history.Count > _options.MaxHistoryItems)
                _history.TryDequeue(out _);
        }

        private void LogStatusChange(
            HealthStatus newStatus,
            IReadOnlyDictionary<string, HealthReportEntry> entries)
        {
            if (_currentStatus == newStatus)
                return;

            _logger.LogWarning(
                "Health status changed from {OldStatus} to {NewStatus}. Healthy: {Healthy}, Degraded: {Degraded}, Unhealthy: {Unhealthy}",
                _currentStatus,
                newStatus,
                entries.Count(x => x.Value.Status == HealthStatus.Healthy),
                entries.Count(x => x.Value.Status == HealthStatus.Degraded),
                entries.Count(x => x.Value.Status == HealthStatus.Unhealthy));
        }

        private static decimal CalculateUptime(IReadOnlyCollection<HealthSnapshot> snapshots)
        {
            if (snapshots.Count == 0)
                return 100m;

            var unhealthyCount = snapshots.Count(x => x.Status == HealthStatus.Unhealthy);

            return 100m - (decimal)unhealthyCount / snapshots.Count * 100m;
        }

        private static TimeSpan CalculateMttr(IEnumerable<HealthSnapshot> snapshots)
        {
            var downtimes = new List<TimeSpan>();
            DateTime? downtimeStart = null;

            foreach (var snapshot in snapshots.OrderBy(x => x.Timestamp))
            {
                if (snapshot.Status == HealthStatus.Unhealthy && downtimeStart == null)
                    downtimeStart = snapshot.Timestamp;

                if (snapshot.Status == HealthStatus.Healthy && downtimeStart.HasValue)
                {
                    downtimes.Add(snapshot.Timestamp - downtimeStart.Value);
                    downtimeStart = null;
                }
            }

            return downtimes.Count > 0
                ? TimeSpan.FromTicks((long)downtimes.Average(x => x.Ticks))
                : TimeSpan.Zero;
        }
    }
}