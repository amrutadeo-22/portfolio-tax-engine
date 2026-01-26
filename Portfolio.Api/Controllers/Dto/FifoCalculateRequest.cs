using Portfolio.Api.Domain.Enums;

namespace Portfolio.Api.Controllers.Dto;
public class FifoCalculateRequest
{
    public Guid PortfolioId { get; set; }
    public List<TransactionDto> Transactions { get; set; } = [];
}

public class TransactionDto
{
    public string AssetSymbol {get; set;} = default!;
    public decimal Quantity {get; set;}
    public decimal Price {get; set;}
    public DateTime TradeDate {get; set;}
    public int Sequence {get; set;}
    public TransactionType Type {get; set;}
    
}