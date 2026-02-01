using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers.Dto;
using Portfolio.Api.Domain.Repositories;
using Portfolio.Api.Domain.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/portfolios")]
public class PortfolioPositionController : ControllerBase
{
    private readonly PortfolioRepository _portfolioRepo;
    private readonly TransactionRepository _transactionRepo;
    private readonly FifoAccountingService _fifoService;

    public PortfolioPositionController(
        PortfolioRepository portfolioRepo,
        TransactionRepository transactionRepo,
        FifoAccountingService fifoService
    )
    {
        _portfolioRepo = portfolioRepo;
        _transactionRepo = transactionRepo;
        _fifoService = fifoService;
    }

    [HttpGet("api/v1/portfolios/{id}/holdings")]
    public IActionResult Holdings(Guid id)
    {
        var transactions = _transactionRepo.GetByPortfolio(id);

        if (!transactions.Any())
            return Ok(new { holdings = new Dictionary<string, decimal>() });

        var result = _fifoService.Calculate(transactions);

        return Ok(result.RemainingHoldingsPerPortfolio[id]);
    }

    [HttpGet("api/v1/portfolios/{id}/pnl")]
    public IActionResult PnL(Guid id)
    {
        var transactions = _transactionRepo.GetByPortfolio(id);

        if (!transactions.Any())
            return Ok(new { realizedPnL = 0m });

        var result = _fifoService.Calculate(transactions);

        return Ok(new
        {
            total = result.TotalRealizedPnL,
            perAsset = result.RealizedPnLPerPortfolio[id]
        });
    }

}
