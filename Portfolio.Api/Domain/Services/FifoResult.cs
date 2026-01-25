namespace Portfolio.Api.Domain.Services;

public class FifoResult
{
    public decimal TotalRealizedPnL { get; private set; }

    public Dictionary<Guid, Dictionary<string, decimal>> RealizedPnLPerPortfolio { get; } = new();
    public Dictionary<Guid, Dictionary<string, decimal>> RemainingHoldingsPerPortfolio { get; } = new();

    public void AddRealizedPnL(Guid portfolioId, string asset, decimal pnl)
    {
        TotalRealizedPnL += pnl;

        if (!RealizedPnLPerPortfolio.ContainsKey(portfolioId))
            RealizedPnLPerPortfolio[portfolioId] = new();

        if (!RealizedPnLPerPortfolio[portfolioId].ContainsKey(asset))
            RealizedPnLPerPortfolio[portfolioId][asset] = 0;

        RealizedPnLPerPortfolio[portfolioId][asset] += pnl;
    }

    public void SetRemaining(Guid portfolioId, string asset, decimal qty)
    {
        if (!RemainingHoldingsPerPortfolio.ContainsKey(portfolioId))
            RemainingHoldingsPerPortfolio[portfolioId] = new();

        RemainingHoldingsPerPortfolio[portfolioId][asset] = qty;
    }
}
