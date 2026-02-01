using Portfolio.Api.Controllers.Dto;

namespace Portfolio.Api.Domain.Services;

public class UnrealizedPnlService
{
    private readonly PositionService _positionService;

    public UnrealizedPnlService(PositionService positionService)
    {
        _positionService = positionService;
    }

    public IEnumerable<UnrealizedPnlResponse> Calculate(
        Guid portfolioId,
        IDictionary<string, decimal> marketPrices)
    {
        var positions = _positionService.GetPositions(portfolioId);

        foreach (var p in positions)
        {
            if (!marketPrices.TryGetValue(p.AssetSymbol, out var marketPrice))
                continue;

            yield return new UnrealizedPnlResponse
            {
                AssetSymbol = p.AssetSymbol,
                Quantity = p.Quantity,
                AveragePrice = p.AveragePrice,
                MarketPrice = marketPrice,
                UnrealizedPnl =
                    (marketPrice - p.AveragePrice) * p.Quantity
            };
        }
    }
}
