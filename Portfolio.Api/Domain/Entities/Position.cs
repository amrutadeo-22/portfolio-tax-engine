namespace Portfolio.Api.Domain.Entities;

public class Position
{
    public string AssetSymbol { get; }
    public decimal Quantity { get; }
    public decimal AveragePrice { get; }

    public Position(string assetSymbol, decimal quantity, decimal averagePrice)
    {
        AssetSymbol = assetSymbol;
        Quantity = quantity;
        AveragePrice = averagePrice;
    }
}
