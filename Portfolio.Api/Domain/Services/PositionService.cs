using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Repositories;

namespace Portfolio.Api.Domain.Services;

public class PositionService
{
    private readonly TransactionRepository _transactionRepo;

    public PositionService(TransactionRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    public IEnumerable<Position> GetPositions(Guid portfolioId)
    {
        var transactions = _transactionRepo
            .GetByPortfolio(portfolioId)
            .OrderBy(t => t.TradeDate)
            .ThenBy(t => t.Sequence);

        return transactions
            .GroupBy(t => t.AssetSymbol)
            .Select(g =>
            {
                decimal qty = 0;
                decimal cost = 0;

                foreach (var t in g)
                {
                    if (t.Type == TransactionType.Buy)
                    {
                        cost += t.Quantity * t.Price;
                        qty += t.Quantity;
                    }
                    else
                    {
                        qty -= t.Quantity;
                    }
                }

                var avg = qty > 0 ? cost / qty : 0;
                return new Position(g.Key, qty, avg);
            })
            .Where(p => p.Quantity != 0);
    }
}
