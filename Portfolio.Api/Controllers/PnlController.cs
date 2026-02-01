using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Domain.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/portfolios/{portfolioId}/pnl/unrealized")]
public class PnlController : ControllerBase
{
    private readonly UnrealizedPnlService _service;

    public PnlController(UnrealizedPnlService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Calculate(
        Guid portfolioId,
        [FromBody] Dictionary<string, decimal> marketPrices)
    {
        var pnl = _service.Calculate(portfolioId, marketPrices);
        return Ok(pnl);
    }
}
