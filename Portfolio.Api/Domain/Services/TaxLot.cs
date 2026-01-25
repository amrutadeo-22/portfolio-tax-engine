namespace Portfolio.Api.Domain.Services;

internal class TaxLot
{
    public decimal QuantityRemaining {get; set;}
    public decimal CostPrice {get;}
    public DateTime AcquisitionDate {get;}
    public string AssetSymbol {get;}

    public TaxLot( string assetSymbol, decimal quantityRemaining, decimal costPrice, DateTime acquisitionDate){
        QuantityRemaining = quantityRemaining;
        CostPrice = costPrice;
        AcquisitionDate = acquisitionDate;
        AssetSymbol = assetSymbol;
    }

    public void Consume(decimal quantity){
        if(quantity > QuantityRemaining){
            throw new InvalidOperationException("Cannot consume more than the remaining quantity in the tax lot.");
        }
        QuantityRemaining -= quantity;
    }
}