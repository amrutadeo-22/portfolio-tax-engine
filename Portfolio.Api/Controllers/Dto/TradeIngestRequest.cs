namespace Portfolio.Api.Controllers.Dto;

public record TradeIngestRequest(Guid PortfolioId, IEnumerable<TransactionDto> Transactions);
