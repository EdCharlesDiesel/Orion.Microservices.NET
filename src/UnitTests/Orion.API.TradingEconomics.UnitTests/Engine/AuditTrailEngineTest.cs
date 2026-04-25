using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine;


public sealed class AuditTrailEngineTests
{
    [Fact]
    public async Task RecordDecisionAsync_WhenBatchSizeReached_FlushesToStorage()
    {
        var storage = new Mock<IAuditStorage>();

        var engine = CreateEngine(storage, batchSize: 1);

        var record = new AuditRecord
        {
            CorrelationId = Guid.NewGuid(),
            SessionId = "session-1",
            Input = new ForexMarketInput { Pair = "EUR/USD" },
            Decision = new TradingDecision
            {
                Direction = "LONG",
                Confidence = 80
            }
        };

        var id = await engine.RecordDecisionAsync(record);

        Assert.NotEqual(Guid.Empty, id);

        storage.Verify(x => x.StoreBatchAsync(
            It.Is<List<AuditEntry>>(entries =>
                entries.Count == 1 &&
                entries[0].RecordType == AuditRecordType.Decision &&
                entries[0].Pair == "EUR/USD" &&
                entries[0].Direction == "LONG" &&
                entries[0].Confidence == 80)),
            Times.Once);
    }

    [Fact]
    public async Task RecordPipelineStepAsync_WhenStepNameMissing_ThrowsArgumentException()
    {
        var engine = CreateEngine(new Mock<IAuditStorage>(), batchSize: 10);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            engine.RecordPipelineStepAsync(Guid.NewGuid(), "", new { Value = 1 }, TimeSpan.FromMilliseconds(5)));
    }

    [Fact]
    public async Task RecordErrorAsync_WhenExceptionIsNull_ThrowsArgumentNullException()
    {
        var engine = CreateEngine(new Mock<IAuditStorage>(), batchSize: 10);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            engine.RecordErrorAsync(Guid.NewGuid(), "RiskEngine", null!));
    }

    [Fact]
    public async Task FlushAsync_WhenStorageFails_RequeuesEntries()
    {
        var storage = new Mock<IAuditStorage>();

        storage.Setup(x => x.StoreBatchAsync(It.IsAny<List<AuditEntry>>()))
            .ThrowsAsync(new InvalidOperationException("Storage failed."));

        var engine = CreateEngine(storage, batchSize: 10);

        await engine.RecordEventAsync(Guid.NewGuid(), "TEST_EVENT");
        await engine.FlushAsync();
        await engine.FlushAsync();

        storage.Verify(x => x.StoreBatchAsync(It.IsAny<List<AuditEntry>>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GenerateComplianceReportAsync_WhenNoDecisions_ReturnsZeroAverageConfidence()
    {
        var storage = new Mock<IAuditStorage>();

        storage.Setup(x => x.QueryAsync(It.IsAny<AuditQuery>()))
            .ReturnsAsync(new AuditQueryResult
            {
                TotalCount = 0,
                Entries = new List<AuditEntry>()
            });

        var engine = CreateEngine(storage, batchSize: 10);

        var report = await engine.GenerateComplianceReportAsync(
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);

        Assert.Equal(0, report.TotalDecisions);
        Assert.Equal(0, report.AverageConfidence);
        Assert.Empty(report.TradesByDirection);
    }

    [Fact]
    public async Task GenerateComplianceReportAsync_GroupsByDecisionDirection()
    {
        var now = DateTime.UtcNow;

        var storage = new Mock<IAuditStorage>();

        storage.Setup(x => x.QueryAsync(It.IsAny<AuditQuery>()))
            .ReturnsAsync(new AuditQueryResult
            {
                TotalCount = 2,
                Entries = new List<AuditEntry>
                {
                    CreateDecisionEntry(now, "LONG", 80),
                    CreateDecisionEntry(now, "SHORT", 60)
                }
            });

        var engine = CreateEngine(storage, batchSize: 10);

        var report = await engine.GenerateComplianceReportAsync(
            now.AddDays(-1),
            now.AddDays(1));

        Assert.Equal(2, report.TotalDecisions);
        Assert.Equal(70, report.AverageConfidence);
        Assert.Equal(1, report.TradesByDirection["LONG"]);
        Assert.Equal(1, report.TradesByDirection["SHORT"]);
    }

    private static AuditTrailEngine CreateEngine(
        Mock<IAuditStorage> storage,
        int batchSize)
    {
        var logger = Mock.Of<ILogger<AuditTrailEngine>>();

        var options = Options.Create(new AuditTrailOptions
        {
            BatchSize = batchSize,
            FlushIntervalSeconds = 300
        });

        return new AuditTrailEngine(logger, options, storage.Object);
    }

    private static AuditEntry CreateDecisionEntry(
        DateTime timestamp,
        string direction,
        decimal confidence)
    {
        return new AuditEntry
        {
            Id = Guid.NewGuid(),
            Timestamp = timestamp,
            RecordType = AuditRecordType.Decision,
            Data = new AuditRecord
            {
                CorrelationId = Guid.NewGuid(),
                Decision = new TradingDecision
                {
                    Direction = direction,
                    Confidence = confidence
                }
            }
        };
    }
}