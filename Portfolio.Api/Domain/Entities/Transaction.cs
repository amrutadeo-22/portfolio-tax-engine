using Portfolio.Api.Domain.Enums;
namespace Portfolio.Api.Domain.Entities;

public class Transaction
{
    public Guid Id {get; private set;}
    public Guid PortfolioId {get; private set;}

    public string AssetSymbol {get; private set;}
    public TransactionType Type {get; private set;}

    public decimal Quantity {get; private set;}
    public decimal Price {get; private set;}
    public DateTime TradeDate {get; private set;}
    public int Sequence { get; private set; }
    private Transaction(){ } 
    private Transaction(
        Guid portfolioId,
        string assetSymbol, 
        TransactionType type,
        decimal quantity,
        decimal price, 
        DateTime tradeDate,
        int sequence
    ){
        if(quantity <= 0){
            throw new ArgumentException("Quantity must be greater than zero.");
        }
        if(price <= 0){
            throw new ArgumentException("Price must be greater than zero.");
        }
        Id = Guid.NewGuid();
        PortfolioId = portfolioId;
        AssetSymbol = assetSymbol;
        Type = type;
        Quantity = quantity;
        Price = price;
        TradeDate = tradeDate;
        Sequence = sequence;

    }
}