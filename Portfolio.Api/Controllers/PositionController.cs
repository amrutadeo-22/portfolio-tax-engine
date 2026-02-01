using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Domain.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/portfolios/{portfolioId}/positions")]
public class PositionsController : ControllerBase
{
    private readonly PositionService _service;

    public PositionsController(PositionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get(Guid portfolioId)
    {
        var positions = _service.GetPositions(portfolioId);
        return Ok(positions);
    }
}
