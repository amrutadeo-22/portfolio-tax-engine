using Portfolio.Api.Domain.Entities;

namespace Portfolio.Api.Domain.Repositories;

public class InMemoryTradeRepository
{
    private readonly Dictionary<Guid, List<Transaction>> _store = new();

    public void Add(Transaction transaction)
    {
        if (!_store.ContainsKey(transaction.PortfolioId))
            _store[transaction.PortfolioId] = new List<Transaction>();

        _store[transaction.PortfolioId].Add(transaction);
    }

    public IReadOnlyList<Transaction> GetByPortfolio(Guid portfolioId)
    {
        if (!_store.ContainsKey(portfolioId))
            return Array.Empty<Transaction>();

        return _store[portfolioId];
    }
}
