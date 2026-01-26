using Xunit;
using Portfolio.Api.Domain.Services;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Exceptions;

namespace Portfolio.Tests;

public class FifoAccountingServiceTests
{
    [Fact]
    public void BuyThenSell_RealizesCorrectPnL()
    {
        var service = new FifoAccountingService();
        var pid = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(pid, "AAPL", TransactionType.Sell, 50, 15, new DateTime(2024,1,2), 2),
        };

        var result = service.Calculate(txns);

        Assert.Equal(250, result.TotalRealizedPnL);
        Assert.Equal(50, result.RemainingHoldingsPerPortfolio[pid]["AAPL"]);
    }

    [Fact]
    public void SellConsumesOldestLotsFirst_FIFO()
    {
        var service = new FifoAccountingService();
        var pid = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 100, 20, new DateTime(2024,2,1), 2),
            Transaction.Create(pid, "AAPL", TransactionType.Sell, 150, 30, new DateTime(2024,3,1), 3),
        };

        var result = service.Calculate(txns);

        // (100 * (30 - 10)) + (50 * (30 - 20)) = 2000 + 500
        Assert.Equal(2500, result.TotalRealizedPnL);
    }

    [Fact]
    public void OversellThrowsDomainException()
    {
        var service = new FifoAccountingService();
        var pid = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 50, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(pid, "AAPL", TransactionType.Sell, 100, 15, new DateTime(2024,1,2), 2),
        };

        Assert.Throws<DomainException>(() => service.Calculate(txns));
    }

    [Fact]
    public void DifferentAssetsHaveIndependentQueues()
    {
        var service = new FifoAccountingService();
        var pid = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(pid, "MSFT", TransactionType.Buy, 100, 20, new DateTime(2024,1,1), 2),
            Transaction.Create(pid, "AAPL", TransactionType.Sell, 50, 15, new DateTime(2024,1,2), 3),
        };

        var result = service.Calculate(txns);

        Assert.Equal(250, result.TotalRealizedPnL);
        Assert.Equal(50, result.RemainingHoldingsPerPortfolio[pid]["AAPL"]);
        Assert.Equal(100, result.RemainingHoldingsPerPortfolio[pid]["MSFT"]);
    }

    [Fact]
    public void DifferentPortfoliosAreIsolated()
    {
        var service = new FifoAccountingService();
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(p1, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(p2, "AAPL", TransactionType.Buy, 100, 20, new DateTime(2024,1,1), 2),
            Transaction.Create(p1, "AAPL", TransactionType.Sell, 50, 15, new DateTime(2024,1,2), 3),
        };

        var result = service.Calculate(txns);

        Assert.Equal(250, result.TotalRealizedPnL);
        Assert.Equal(50, result.RemainingHoldingsPerPortfolio[p1]["AAPL"]);
        Assert.Equal(100, result.RemainingHoldingsPerPortfolio[p2]["AAPL"]);
    }

    [Fact]
    public void SellingExactQuantityClearsLotsCompletely()
    {
        var service = new FifoAccountingService();
        var pid = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(pid, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(pid, "AAPL", TransactionType.Sell, 100, 15, new DateTime(2024,1,2), 2),
        };

        var result = service.Calculate(txns);

        Assert.Equal(500, result.TotalRealizedPnL);
        Assert.False(result.RemainingHoldingsPerPortfolio[pid].ContainsKey("AAPL"));
    }
    [Fact]
    public void EmptyTransactionList_ThrowsDomainException()
    {
        var service = new FifoAccountingService();

        var txns = Array.Empty<Transaction>();

        var ex = Assert.Throws<DomainException>(() => service.Calculate(txns));
        Assert.Equal("No transactions provided.", ex.Message);
    }
    [Fact]
    public void MultiplePortfolios_AreCalculatedIndependently()
    {
        var service = new FifoAccountingService();
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        var txns = new[]
        {
            Transaction.Create(p1, "AAPL", TransactionType.Buy, 100, 10, new DateTime(2024,1,1), 1),
            Transaction.Create(p1, "AAPL", TransactionType.Sell, 50, 20, new DateTime(2024,1,2), 2),

            Transaction.Create(p2, "AAPL", TransactionType.Buy, 200, 5, new DateTime(2024,1,1), 1),
            Transaction.Create(p2, "AAPL", TransactionType.Sell, 100, 15, new DateTime(2024,1,2), 2),
        };

        var result = service.Calculate(txns);

        Assert.Equal(500, result.RealizedPnLPerPortfolio[p1]["AAPL"]); // (20-10)*50
        Assert.Equal(1000, result.RealizedPnLPerPortfolio[p2]["AAPL"]); // (15-5)*100

        Assert.Equal(50, result.RemainingHoldingsPerPortfolio[p1]["AAPL"]);
        Assert.Equal(100, result.RemainingHoldingsPerPortfolio[p2]["AAPL"]);
    }


}
