using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Exceptions;
namespace Portfolio.Api.Domain.Repositories;

public class TransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public void Add(Transaction transaction)
    {
        var exists  = _transactions.Any(
            t=> t.PortfolioId == transaction.PortfolioId &&
                    t.TradeDate == transaction.TradeDate &&
                    t.Sequence == transaction.Sequence
        );
        if (exists)
        {
            throw new DomainException(
                $"Duplicate transaction sequence {transaction.Sequence} for portfolio {transaction.PortfolioId} on {transaction.TradeDate:yyyy-MM-dd}"
            );
        }   
        _transactions.Add(transaction);
    }

    public IEnumerable<Transaction> GetByPortfolio(Guid portfolioId)
    {
        return _transactions.Where(t => t.PortfolioId == portfolioId).ToList();
    }
}
