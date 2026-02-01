using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers.Dto;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Repositories;
using PortfolioEntity = Portfolio.Api.Domain.Entities.Portfolio;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v1/portfolios")]
public class PortfolioController : ControllerBase
{
    private readonly PortfolioRepository _portfolioRepo;

    public PortfolioController(PortfolioRepository portfolioRepo)
    {
        _portfolioRepo = portfolioRepo;
    }

    [HttpPost]
    public IActionResult Create([FromBody] PortfolioCreateDto request)
    {
        var portfolio = PortfolioEntity.Create(request.Name);
        _portfolioRepo.Create(portfolio);
        return Ok(new PortfolioResponseDto(portfolio.Id, portfolio.Name));
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var portfolio = _portfolioRepo.Get(id);
        if (portfolio == null) return NotFound();
        return Ok(new PortfolioResponseDto(portfolio.Id, portfolio.Name));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var list = _portfolioRepo.GetAll()
            .Select(p => new PortfolioResponseDto(p.Id, p.Name));
        return Ok(list);
    }
}
