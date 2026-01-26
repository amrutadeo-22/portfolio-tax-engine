using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers.Dto;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Exceptions;
using Portfolio.Api.Domain.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/fifo")]
public class FifoController : ControllerBase
{
    private readonly FifoAccountingService _fifoService;

    public FifoController(FifoAccountingService fifoService)
    {
        _fifoService = fifoService;
    }

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] FifoCalculateRequest request)
    {
        if (request.Transactions == null || request.Transactions.Count == 0)
        {
            return BadRequest(new { error = "At least one transaction is required." });
        }

        if (request.Transactions.Any(t => string.IsNullOrWhiteSpace(t.AssetSymbol)))
        {
            return BadRequest(new { error = "AssetSymbol is required for all transactions." });
        }
        
        try
        {
            var transactions = request.Transactions.Select(t =>
                Transaction.Create(
                    request.PortfolioId,
                    t.AssetSymbol,
                    t.Type,
                    t.Quantity,
                    t.Price,
                    t.TradeDate,
                    t.Sequence
                )
            );

            var result = _fifoService.Calculate(transactions);

            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
