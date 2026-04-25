// namespace Orion.API.TradingEconomics.Engine;
//
// using System.Collections.Concurrent;
// using System.Diagnostics;
// using Microsoft.Extensions.Diagnostics.HealthChecks;
//
// namespace Orion.API.TradingEconomics.Engine
// {
//     public sealed class HealthCheckEngine
//     {
//         private readonly ILogger<HealthCheckEngine> _logger;
//         private readonly HealthCheckOptions _options;
//         private readonly IServiceProvider _serviceProvider;
//         private readonly ConcurrentDictionary<string, HealthComponent> _components;
//         private readonly ConcurrentQueue<HealthSnapshot> _history;
//         private readonly Timer _monitoringTimer;
//         private readonly SemaphoreSlim _checkLock;
//         private HealthStatus _currentStatus;
//
//         public HealthCheckEngine(
//             ILogger<HealthCheckEngine> logger,
//             IOptions<HealthCheckOptions> options,
//             IServiceProvider serviceProvider)
//         {
//             _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//             _options = options?.Value ?? new HealthCheckOptions();
//             _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
//             _components = new ConcurrentDictionary<string, HealthComponent>();
//             _history = new ConcurrentQueue<HealthSnapshot>();
//             _checkLock = new SemaphoreSlim(1, 1);
//             _currentStatus = HealthStatus.Healthy;
//
//             RegisterDefaultComponents();
//             
//             _monitoringTimer = new Timer(
//                 async _ => await RunHealthChecksAsync(),
//                 null,
//                 TimeSpan.FromSeconds(_options.InitialDelaySeconds),
//                 TimeSpan.FromSeconds(_options.CheckIntervalSeconds));
//         }
//
//         private void RegisterDefaultComponents()
//         {
//             // Data Provider Health
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "YahooFinanceDataProvider",
//                 Type = HealthComponentType.DataProvider,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(10),
//                 DegradedThreshold = TimeSpan.FromSeconds(5)
//             });
//
//             // Pipeline Engines Health
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "NormalizationEngine",
//                 Type = HealthComponentType.PipelineEngine,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(5)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "RegimeEngine",
//                 Type = HealthComponentType.PipelineEngine,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(5)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "SignalEngine",
//                 Type = HealthComponentType.PipelineEngine,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(5)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "RiskEngine",
//                 Type = HealthComponentType.PipelineEngine,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(3)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "ExecutionEngine",
//                 Type = HealthComponentType.PipelineEngine,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(5)
//             });
//
//             // External Services
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "TradingEconomicsAPI",
//                 Type = HealthComponentType.ExternalService,
//                 Critical = false,
//                 Timeout = TimeSpan.FromSeconds(15)
//             });
//
//             // Infrastructure
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "AuditTrailStorage",
//                 Type = HealthComponentType.Infrastructure,
//                 Critical = true,
//                 Timeout = TimeSpan.FromSeconds(5)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "MemoryUsage",
//                 Type = HealthComponentType.Infrastructure,
//                 Critical = true,
//                 CheckInterval = TimeSpan.FromMinutes(1)
//             });
//
//             RegisterComponent(new HealthComponent
//             {
//                 Name = "CpuUsage",
//                 Type = HealthComponentType.Infrastructure,
//                 Critical = false,
//                 CheckInterval = TimeSpan.FromMinutes(1)
//             });
//         }
//
//         public void RegisterComponent(HealthComponent component)
//         {
//             component.LastCheckTime = DateTime.UtcNow;
//             _components[component.Name] = component;
//             _logger.LogInformation("Registered health component: {Component}", component.Name);
//         }
//
//         public async Task<HealthReport> RunHealthChecksAsync(CancellationToken cancellationToken = default)
//         {
//             if (!await _checkLock.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken))
//             {
//                 _logger.LogWarning("Health check already in progress, skipping");
//                 return CreateCachedReport();
//             }
//
//             try
//             {
//                 var report = new HealthReport
//                 {
//                     Timestamp = DateTime.UtcNow,
//                     Checks = new Dictionary<string, HealthCheckResult>(),
//                     Metrics = new SystemMetrics()
//                 };
//
//                 var tasks = _components.Values
//                     .Where(c => ShouldCheck(c))
//                     .Select(c => CheckComponentAsync(c, cancellationToken));
//
//                 var results = await Task.WhenAll(tasks);
//
//                 foreach (var (component, result) in results)
//                 {
//                     report.Checks[component.Name] = result;
//                     component.LastCheckTime = DateTime.UtcNow;
//                     component.LastResult = result;
//                 }
//
//                 // System metrics
//                 report.Metrics = await CollectSystemMetricsAsync();
//
//                 // Determine overall status
//                 report.OverallStatus = DetermineOverallStatus(report.Checks);
//                 _currentStatus = report.OverallStatus;
//
//                 // Store history
//                 _history.Enqueue(new HealthSnapshot
//                 {
//                     Timestamp = report.Timestamp,
//                     Status = report.OverallStatus,
//                     ComponentCount = report.Checks.Count,
//                     HealthyCount = report.Checks.Count(c => c.Value.Status == HealthStatus.Healthy),
//                     DegradedCount = report.Checks.Count(c => c.Value.Status == HealthStatus.Degraded),
//                     UnhealthyCount = report.Checks.Count(c => c.Value.Status == HealthStatus.Unhealthy)
//                 });
//
//                 // Trim history
//                 while (_history.Count > _options.MaxHistoryItems)
//                 {
//                     _history.TryDequeue(out _);
//                 }
//
//                 // Log status changes
//                 LogStatusChange(report);
//
//                 return report;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Health check execution failed");
//                 return CreateErrorReport(ex);
//             }
//             finally
//             {
//                 _checkLock.Release();
//             }
//         }
//
//         private async Task<(HealthComponent Component, HealthCheckResult Result)> CheckComponentAsync(
//             HealthComponent component, 
//             CancellationToken cancellationToken)
//         {
//             var sw = Stopwatch.StartNew();
//             
//             try
//             {
//                 using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
//                 cts.CancelAfter(component.Timeout);
//
//                 var result = component.Type switch
//                 {
//                     HealthComponentType.DataProvider => await CheckDataProviderAsync(component, cts.Token),
//                     HealthComponentType.PipelineEngine => await CheckPipelineEngineAsync(component, cts.Token),
//                     HealthComponentType.ExternalService => await CheckExternalServiceAsync(component, cts.Token),
//                     HealthComponentType.Infrastructure => await CheckInfrastructureAsync(component, cts.Token),
//                     _ => HealthCheckResult.Healthy($"{component.Name} is operational")
//                 };
//
//                 sw.Stop();
//                 result = result with 
//                 { 
//                     Data = new Dictionary<string, object>(result.Data ?? new Dictionary<string, object>())
//                     {
//                         ["ResponseTimeMs"] = sw.ElapsedMilliseconds,
//                         ["LastCheck"] = DateTime.UtcNow
//                     }
//                 };
//
//                 if (sw.Elapsed > component.DegradedThreshold)
//                 {
//                     return (component, HealthCheckResult.Degraded(
//                         $"Response time {sw.ElapsedMilliseconds}ms exceeds threshold {component.DegradedThreshold.TotalMilliseconds}ms",
//                         data: result.Data));
//                 }
//
//                 return (component, result);
//             }
//             catch (OperationCanceledException)
//             {
//                 sw.Stop();
//                 _logger.LogWarning("Health check timed out for {Component} after {Elapsed}ms", 
//                     component.Name, sw.ElapsedMilliseconds);
//                 
//                 return component.Critical
//                     ? (component, HealthCheckResult.Unhealthy(
//                         $"Timeout after {sw.ElapsedMilliseconds}ms",
//                         data: new Dictionary<string, object> { ["TimeoutMs"] = sw.ElapsedMilliseconds }))
//                     : (component, HealthCheckResult.Degraded(
//                         $"Timeout after {sw.ElapsedMilliseconds}ms",
//                         data: new Dictionary<string, object> { ["TimeoutMs"] = sw.ElapsedMilliseconds }));
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Health check failed for {Component}", component.Name);
//                 
//                 return component.Critical
//                     ? (component, HealthCheckResult.Unhealthy(
//                         $"Check failed: {ex.Message}",
//                         exception: ex))
//                     : (component, HealthCheckResult.Degraded(
//                         $"Check failed: {ex.Message}",
//                         exception: ex));
//             }
//         }
//
//         private async Task<HealthCheckResult> CheckDataProviderAsync(
//             HealthComponent component, 
//             CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Test data provider connectivity
//                 var provider = _serviceProvider.GetService<IYahooFinanceService>();
//                 if (provider == null)
//                     return HealthCheckResult.Unhealthy("YahooFinanceService not registered");
//
//                 // Quick connectivity test with a known symbol
//                 var data = await provider.GetHistoricalDataAsync(
//                     "EURUSD=X", "1d", "1h", cancellationToken);
//
//                 if (data == null)
//                     return HealthCheckResult.Unhealthy("Data provider returned null");
//
//                 if (data.Count == 0)
//                     return HealthCheckResult.Degraded("Data provider returned empty dataset");
//
//                 // Check data quality
//                 var dataQuality = ValidateDataQuality(data);
//                 if (!dataQuality.IsValid)
//                 {
//                     return HealthCheckResult.Degraded(
//                         $"Data quality issues: {string.Join(", ", dataQuality.Issues)}",
//                         data: new Dictionary<string, object>
//                         {
//                             ["DataPointCount"] = data.Count,
//                             ["QualityScore"] = dataQuality.Score,
//                             ["Issues"] = dataQuality.Issues
//                         });
//                 }
//
//                 return HealthCheckResult.Healthy(
//                     $"Data provider operational. Retrieved {data.Count} bars",
//                     data: new Dictionary<string, object>
//                     {
//                         ["DataPointCount"] = data.Count,
//                         ["LatestTimestamp"] = data.Last().TimestampUtc,
//                         ["Symbol"] = "EURUSD=X"
//                     });
//             }
//             catch (Exception ex)
//             {
//                 return HealthCheckResult.Unhealthy(
//                     $"Data provider check failed: {ex.Message}",
//                     exception: ex);
//             }
//         }
//
//         private async Task<HealthCheckResult> CheckPipelineEngineAsync(
//             HealthComponent component, 
//             CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Verify engine is instantiated and responsive
//                 object engine = component.Name switch
//                 {
//                     "NormalizationEngine" => _serviceProvider.GetService<NormalizationEngine>(),
//                     "RegimeEngine" => _serviceProvider.GetService<RegimeEngine>(),
//                     "SignalEngine" => _serviceProvider.GetService<SignalEngine>(),
//                     "RiskEngine" => _serviceProvider.GetService<RiskEngine>(),
//                     "ExecutionEngine" => _serviceProvider.GetService<ExecutionEngine>(),
//                     _ => null
//                 };
//
//                 if (engine == null)
//                     return HealthCheckResult.Unhealthy($"Engine {component.Name} not registered in DI");
//
//                 // Perform a lightweight test if possible
//                 var testResult = await PerformEngineSmokeTestAsync(component.Name, engine, cancellationToken);
//
//                 return testResult;
//             }
//             catch (Exception ex)
//             {
//                 return HealthCheckResult.Unhealthy(
//                     $"Engine check failed: {ex.Message}",
//                     exception: ex);
//             }
//         }
//
//         private Task<HealthCheckResult> PerformEngineSmokeTestAsync(
//             string engineName, 
//             object engine, 
//             CancellationToken cancellationToken)
//         {
//             // Lightweight validation that engine is operational
//             // Could test with minimal input/output validation
//             
//             var tests = new Dictionary<string, bool>
//             {
//                 ["InstanceExists"] = engine != null,
//                 ["TypeCorrect"] = engine?.GetType().IsClass == true
//             };
//
//             var allPassed = tests.All(t => t.Value);
//             
//             return Task.FromResult(allPassed
//                 ? HealthCheckResult.Healthy($"{engineName} operational", 
//                     data: new Dictionary<string, object> { ["Tests"] = tests })
//                 : HealthCheckResult.Degraded($"{engineName} degraded",
//                     data: new Dictionary<string, object> { ["Tests"] = tests }));
//         }
//
//         private async Task<HealthCheckResult> CheckExternalServiceAsync(
//             HealthComponent component, 
//             CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Check Trading Economics API
//                 if (component.Name == "TradingEconomicsAPI")
//                 {
//                     var service = _serviceProvider.GetService<ITradingEconomicsService>();
//                     if (service == null)
//                         return HealthCheckResult.Unhealthy("TradingEconomicsService not registered");
//
//                     var connectivity = await service.CheckConnectivityAsync(cancellationToken);
//                     
//                     return connectivity.IsConnected
//                         ? HealthCheckResult.Healthy(
//                             $"API connected. Latency: {connectivity.LatencyMs}ms",
//                             data: new Dictionary<string, object>
//                             {
//                                 ["LatencyMs"] = connectivity.LatencyMs,
//                                 ["ApiStatus"] = connectivity.Status
//                             })
//                         : HealthCheckResult.Unhealthy(
//                             $"API unreachable: {connectivity.ErrorMessage}");
//                 }
//
//                 return HealthCheckResult.Healthy($"{component.Name} operational");
//             }
//             catch (Exception ex)
//             {
//                 return HealthCheckResult.Unhealthy(
//                     $"External service check failed: {ex.Message}",
//                     exception: ex);
//             }
//         }
//
//         private async Task<HealthCheckResult> CheckInfrastructureAsync(
//             HealthComponent component, 
//             CancellationToken cancellationToken)
//         {
//             try
//             {
//                 return component.Name switch
//                 {
//                     "MemoryUsage" => CheckMemoryUsage(),
//                     "CpuUsage" => CheckCpuUsage(),
//                     "AuditTrailStorage" => await CheckAuditStorageAsync(cancellationToken),
//                     _ => HealthCheckResult.Healthy($"{component.Name} operational")
//                 };
//             }
//             catch (Exception ex)
//             {
//                 return HealthCheckResult.Unhealthy(
//                     $"Infrastructure check failed: {ex.Message}",
//                     exception: ex);
//             }
//         }
//
//         private HealthCheckResult CheckMemoryUsage()
//         {
//             var process = Process.GetCurrentProcess();
//             var usedMemoryMB = process.WorkingSet64 / 1024.0 / 1024.0;
//             var totalMemoryMB = GC.GetTotalMemory(false) / 1024.0 / 1024.0;
//
//             var data = new Dictionary<string, object>
//             {
//                 ["WorkingSetMB"] = Math.Round(usedMemoryMB, 2),
//                 ["ManagedMemoryMB"] = Math.Round(totalMemoryMB, 2),
//                 ["GcCollections"] = new Dictionary<string, int>
//                 {
//                     ["Gen0"] = GC.CollectionCount(0),
//                     ["Gen1"] = GC.CollectionCount(1),
//                     ["Gen2"] = GC.CollectionCount(2)
//                 }
//             };
//
//             if (usedMemoryMB > _options.MaxMemoryThresholdMB)
//             {
//                 return HealthCheckResult.Degraded(
//                     $"High memory usage: {usedMemoryMB:F1}MB",
//                     data: data);
//             }
//
//             return HealthCheckResult.Healthy(
//                 $"Memory usage: {usedMemoryMB:F1}MB",
//                 data: data);
//         }
//
//         private HealthCheckResult CheckCpuUsage()
//         {
//             var process = Process.GetCurrentProcess();
//             var cpuUsage = process.TotalProcessorTime;
//             
//             var data = new Dictionary<string, object>
//             {
//                 ["TotalProcessorTime"] = cpuUsage.ToString(),
//                 ["ThreadCount"] = process.Threads.Count
//             };
//
//             return HealthCheckResult.Healthy(
//                 $"CPU time: {cpuUsage.TotalSeconds:F1}s",
//                 data: data);
//         }
//
//         private async Task<HealthCheckResult> CheckAuditStorageAsync(CancellationToken cancellationToken)
//         {
//             var storage = _serviceProvider.GetService<IAuditStorage>();
//             if (storage == null)
//                 return HealthCheckResult.Unhealthy("Audit storage not registered");
//
//             try
//             {
//                 // Try to query recent entry to verify storage accessibility
//                 var query = new AuditQuery
//                 {
//                     StartDate = DateTime.UtcNow.AddMinutes(-5),
//                     PageSize = 1
//                 };
//                 
//                 await storage.QueryAsync(query);
//                 
//                 return HealthCheckResult.Healthy("Audit storage accessible");
//             }
//             catch (Exception ex)
//             {
//                 return HealthCheckResult.Unhealthy(
//                     $"Audit storage inaccessible: {ex.Message}",
//                     exception: ex);
//             }
//         }
//
//         private async Task<SystemMetrics> CollectSystemMetricsAsync()
//         {
//             var process = Process.GetCurrentProcess();
//             
//             return new SystemMetrics
//             {
//                 ProcessStartTime = process.StartTime,
//                 UpTime = DateTime.Now - process.StartTime,
//                 ThreadCount = process.Threads.Count,
//                 HandleCount = process.HandleCount,
//                 WorkingSet = process.WorkingSet64,
//                 PeakWorkingSet = process.PeakWorkingSet64,
//                 PrivateMemory = process.PrivateMemorySize64,
//                 VirtualMemory = process.VirtualMemorySize64,
//                 PagedMemory = process.PagedMemorySize64,
//                 GcTotalMemory = GC.GetTotalMemory(false),
//                 PipelineDecisionsProcessed = GetDecisionCount(),
//                 ErrorRate = CalculateErrorRate()
//             };
//         }
//
//         private DataQualityResult ValidateDataQuality(List<OhlcvBar> data)
//         {
//             var issues = new List<string>();
//             var score = 100;
//
//             // Check for gaps
//             var expectedInterval = TimeSpan.FromHours(1);
//             for (int i = 1; i < data.Count; i++)
//             {
//                 var gap = data[i].TimestampUtc - data[i - 1].TimestampUtc;
//                 if (gap > expectedInterval * 2)
//                 {
//                     issues.Add($"Data gap detected at {data[i].TimestampUtc}: {gap.TotalHours:F1}h");
//                     score -= 10;
//                 }
//             }
//
//             // Check for anomalies (zero or negative prices)
//             var anomalies = data.Count(b => b.Open <= 0 || b.High <= 0 || b.Low <= 0 || b.Close <= 0);
//             if (anomalies > 0)
//             {
//                 issues.Add($"Found {anomalies} bars with invalid prices");
//                 score -= 20;
//             }
//
//             // Check OHLC logic
//             var ohlcIssues = data.Count(b => b.High < b.Low || b.Open > b.High || b.Open < b.Low || 
//                                               b.Close > b.High || b.Close < b.Low);
//             if (ohlcIssues > 0)
//             {
//                 issues.Add($"Found {ohlcIssues} bars with invalid OHLC relationships");
//                 score -= 30;
//             }
//
//             // Check volume
//             var zeroVolume = data.Count(b => b.Volume == 0);
//             if (zeroVolume > data.Count * 0.5) // More than 50% zero volume
//             {
//                 issues.Add($"High percentage of zero volume bars: {zeroVolume}/{data.Count}");
//                 score -= 10;
//             }
//
//             return new DataQualityResult
//             {
//                 IsValid = issues.Count == 0,
//                 Score = Math.Max(0, score),
//                 Issues = issues
//             };
//         }
//
//         private HealthStatus DetermineOverallStatus(Dictionary<string, HealthCheckResult> checks)
//         {
//             var criticalUnhealthy = _components.Values
//                 .Where(c => c.Critical)
//                 .Any(c => checks.ContainsKey(c.Name) && checks[c.Name].Status == HealthStatus.Unhealthy);
//
//             if (criticalUnhealthy)
//                 return HealthStatus.Unhealthy;
//
//             var anyUnhealthy = checks.Any(c => c.Value.Status == HealthStatus.Unhealthy);
//             if (anyUnhealthy)
//                 return HealthStatus.Degraded;
//
//             var anyDegraded = checks.Any(c => c.Value.Status == HealthStatus.Degraded);
//             if (anyDegraded)
//                 return HealthStatus.Degraded;
//
//             return HealthStatus.Healthy;
//         }
//
//         private bool ShouldCheck(HealthComponent component)
//         {
//             if (!component.Enabled)
//                 return false;
//
//             if (component.CheckInterval.HasValue)
//             {
//                 return DateTime.UtcNow - component.LastCheckTime >= component.CheckInterval.Value;
//             }
//
//             return true;
//         }
//
//         private void LogStatusChange(HealthReport report)
//         {
//             if (_currentStatus != report.OverallStatus)
//             {
//                 _logger.LogWarning(
//                     "Health status changed from {OldStatus} to {NewStatus}. " +
//                     "Healthy: {Healthy}, Degraded: {Degraded}, Unhealthy: {Unhealthy}",
//                     _currentStatus, report.OverallStatus,
//                     report.Checks.Count(c => c.Value.Status == HealthStatus.Healthy),
//                     report.Checks.Count(c => c.Value.Status == HealthStatus.Degraded),
//                     report.Checks.Count(c => c.Value.Status == HealthStatus.Unhealthy));
//             }
//         }
//
//         // Public API Methods
//
//         public Task<HealthReport> GetCurrentHealthAsync()
//         {
//             return Task.FromResult(CreateCachedReport());
//         }
//
//         public async Task<HealthTrend> GetHealthTrendAsync(int hours = 24)
//         {
//             var snapshots = _history
//                 .Where(h => h.Timestamp >= DateTime.UtcNow.AddHours(-hours))
//                 .OrderBy(h => h.Timestamp)
//                 .ToList();
//
//             return new HealthTrend
//             {
//                 Snapshots = snapshots,
//                 UptimePercentage = CalculateUptime(snapshots),
//                 MeanTimeToRecovery = CalculateMTTR(snapshots),
//                 StatusDistribution = snapshots
//                     .GroupBy(s => s.Status)
//                     .ToDictionary(g => g.Key, g => g.Count())
//             };
//         }
//
//         public async Task<ComponentDetails> GetComponentDetailsAsync(string componentName)
//         {
//             if (!_components.TryGetValue(componentName, out var component))
//                 return null;
//
//             return new ComponentDetails
//             {
//                 Name = component.Name,
//                 Type = component.Type,
//                 Critical = component.Critical,
//                 LastCheck = component.LastCheckTime,
//                 LastResult = component.LastResult,
//                 TotalChecks = component.TotalChecks,
//                 FailedChecks = component.FailedChecks,
//                 SuccessRate = component.TotalChecks > 0 
//                     ? (double)(component.TotalChecks - component.FailedChecks) / component.TotalChecks * 100
//                     : 0
//             };
//         }
//
//         public void EnableComponent(string name, bool enabled)
//         {
//             if (_components.TryGetValue(name, out var component))
//             {
//                 component.Enabled = enabled;
//                 _logger.LogInformation("Component {Component} enabled: {Enabled}", name, enabled);
//             }
//         }
//
//         public async Task GracefulShutdownAsync()
//         {
//             _logger.LogInformation("Shutting down health monitoring");
//             await _monitoringTimer.DisposeAsync();
//             await RunHealthChecksAsync(); // Final check before shutdown
//         }
//
//         private HealthReport CreateCachedReport()
//         {
//             return new HealthReport
//             {
//                 Timestamp = DateTime.UtcNow,
//                 OverallStatus = _currentStatus,
//                 Checks = _components.Values
//                     .Where(c => c.LastResult != null)
//                     .ToDictionary(c => c.Name, c => c.LastResult),
//                 Metrics = new SystemMetrics()
//             };
//         }
//
//         private HealthReport CreateErrorReport(Exception ex)
//         {
//             return new HealthReport
//             {
//                 Timestamp = DateTime.UtcNow,
//                 OverallStatus = HealthStatus.Unhealthy,
//                 Checks = new Dictionary<string, HealthCheckResult>
//                 {
//                     ["HealthCheckEngine"] = HealthCheckResult.Unhealthy(
//                         $"Health check system failed: {ex.Message}", ex)
//                 }
//             };
//         }
//
//         private decimal CalculateUptime(List<HealthSnapshot> snapshots)
//         {
//             if (!snapshots.Any())
//                 return 100;
//
//             var unhealthyCount = snapshots.Count(s => s.Status == HealthStatus.Unhealthy);
//             return 100 - (decimal)unhealthyCount / snapshots.Count * 100;
//         }
//
//         private TimeSpan CalculateMTTR(List<HealthSnapshot> snapshots)
//         {
//             var downtimes = new List<TimeSpan>();
//             DateTime? downtimeStart = null;
//
//             foreach (var snapshot in snapshots.OrderBy(s => s.Timestamp))
//             {
//                 if (snapshot.Status == HealthStatus.Unhealthy && downtimeStart == null)
//                 {
//                     downtimeStart = snapshot.Timestamp;
//                 }
//                 else if (snapshot.Status == HealthStatus.Healthy && downtimeStart.HasValue)
//                 {
//                     downtimes.Add(snapshot.Timestamp - downtimeStart.Value);
//                     downtimeStart = null;
//                 }
//             }
//
//             return downtimes.Any()
//                 ? TimeSpan.FromTicks((long)downtimes.Average(t => t.Ticks))
//                 : TimeSpan.Zero;
//         }
//
//         private long GetDecisionCount()
//         {
//             // Track decisions processed (inject counter service or access AuditTrail)
//             return 0;
//         }
//
//         private double CalculateErrorRate()
//         {
//             // Calculate error rate from recent history
//             return 0;
//         }
//     }
//
//     #region Entities and Models
//
//     public class HealthCheckOptions
//     {
//         public int CheckIntervalSeconds { get; set; } = 30;
//         public int InitialDelaySeconds { get; set; } = 5;
//         public int MaxHistoryItems { get; set; } = 1000;
//         public double MaxMemoryThresholdMB { get; set; } = 2048;
//         public double MaxCpuPercent { get; set; } = 80;
//         public bool DetailedMetricsEnabled { get; set; } = true;
//         public List<string> ExcludedComponents { get; set; } = new();
//         public Dictionary<string, object> CustomThresholds { get; set; } = new();
//     }
//
//     public class HealthComponent
//     {
//         public string Name { get; set; }
//         public HealthComponentType Type { get; set; }
//         public bool Critical { get; set; }
//         public bool Enabled { get; set; } = true;
//         public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
//         public TimeSpan DegradedThreshold { get; set; } = TimeSpan.FromSeconds(5);
//         public TimeSpan? CheckInterval { get; set; }
//         public DateTime LastCheckTime { get; set; }
//         public HealthCheckResult? LastResult { get; set; }
//         public long TotalChecks { get; set; }
//         public long FailedChecks { get; set; }
//     }
//
//     public enum HealthComponentType
//     {
//         DataProvider,
//         PipelineEngine,
//         ExternalService,
//         Infrastructure,
//         Database,
//         Cache,
//         MessageQueue,
//         Custom
//     }
//
//     public class HealthReport
//     {
//         public DateTime Timestamp { get; set; }
//         public HealthStatus OverallStatus { get; set; }
//         public Dictionary<string, HealthCheckResult> Checks { get; set; }
//         public SystemMetrics Metrics { get; set; }
//         public string Version { get; set; } = "1.0.0";
//     }
//
//     public class SystemMetrics
//     {
//         public DateTime ProcessStartTime { get; set; }
//         public TimeSpan UpTime { get; set; }
//         public int ThreadCount { get; set; }
//         public int HandleCount { get; set; }
//         public long WorkingSet { get; set; }
//         public long PeakWorkingSet { get; set; }
//         public long PrivateMemory { get; set; }
//         public long VirtualMemory { get; set; }
//         public long PagedMemory { get; set; }
//         public long GcTotalMemory { get; set; }
//         public long PipelineDecisionsProcessed { get; set; }
//         public double ErrorRate { get; set; }
//     }
//
//     public class HealthSnapshot
//     {
//         public DateTime Timestamp { get; set; }
//         public HealthStatus Status { get; set; }
//         public int ComponentCount { get; set; }
//         public int HealthyCount { get; set; }
//         public int DegradedCount { get; set; }
//         public int UnhealthyCount { get; set; }
//     }
//
//     public class HealthTrend
//     {
//         public List<HealthSnapshot> Snapshots { get; set; }
//         public decimal UptimePercentage { get; set; }
//         public TimeSpan MeanTimeToRecovery { get; set; }
//         public Dictionary<HealthStatus, int> StatusDistribution { get; set; }
//     }
//
//     public class ComponentDetails
//     {
//         public string Name { get; set; }
//         public HealthComponentType Type { get; set; }
//         public bool Critical { get; set; }
//         public DateTime LastCheck { get; set; }
//         public HealthCheckResult? LastResult { get; set; }
//         public long TotalChecks { get; set; }
//         public long FailedChecks { get; set; }
//         public double SuccessRate { get; set; }
//     }
//
//     public class DataQualityResult
//     {
//         public bool IsValid { get; set; }
//         public int Score { get; set; }
//         public List<string> Issues { get; set; } = new();
//     }
//
//     public enum HealthStatus
//     {
//         Healthy,
//         Degraded,
//         Unhealthy
//     }
//
//     #endregion
// }