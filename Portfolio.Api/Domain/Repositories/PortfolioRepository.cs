using PortfolioEntity = Portfolio.Api.Domain.Entities.Portfolio;


namespace Portfolio.Api.Domain.Repositories;

public class PortfolioRepository
{
    private readonly Dictionary<Guid, PortfolioEntity> _portfolios = new();

    public PortfolioEntity Create(PortfolioEntity portfolio)
    {
        _portfolios.Add(portfolio.Id, portfolio);
        return portfolio;
    }

    public PortfolioEntity? Get(Guid id)
    {
        return _portfolios.TryGetValue(id, out var portfolio) ? portfolio : null;
    }

    public IEnumerable<PortfolioEntity> GetAll()
    {
        return _portfolios.Values;
    }
}
