namespace Portfolio.Api.Controllers.Dto;

public class UnrealizedPnlResponse
{
    public string AssetSymbol { get; set; } = default!;
    public decimal Quantity { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal MarketPrice { get; set; }
    public decimal UnrealizedPnl { get; set; }
}
