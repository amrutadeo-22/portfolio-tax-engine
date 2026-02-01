using Portfolio.Api.Domain.Entities;

namespace Portfolio.Api.Domain.Repositories;

public class TransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public void Add(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public IEnumerable<Transaction> GetByPortfolio(Guid portfolioId)
    {
        return _transactions.Where(t => t.PortfolioId == portfolioId);
    }
}
