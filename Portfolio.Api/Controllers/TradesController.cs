using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers.Dto;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Exceptions;
using Portfolio.Api.Domain.Repositories;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/trades")]
public class TradesController : ControllerBase
{
    private readonly TransactionRepository _transactionRepo;

    public TradesController(TransactionRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    [HttpPost]
    public IActionResult Ingest([FromBody] TradeIngestRequest request)
    {
        if (request.Transactions == null || !request.Transactions.Any())
            return BadRequest(new { error = "Transactions cannot be empty." });

        try
        {
            foreach (var t in request.Transactions)
            {
                _transactionRepo.Add(Transaction.Create(
                    request.PortfolioId,
                    t.AssetSymbol,
                    t.Type,
                    t.Quantity,
                    t.Price,
                    t.TradeDate,
                    t.Sequence
                ));
            }
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }


        return Created("", new { status = "ok" });
    }
}
