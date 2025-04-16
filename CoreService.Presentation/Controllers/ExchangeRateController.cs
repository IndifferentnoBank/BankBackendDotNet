using System.IdentityModel.Tokens.Jwt;
using CoreService.Application.Features.Queries.GetExchangeRate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
[Route("core_service/exchange_rates")]
public class ExchangeRateController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangeRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetExchangeRates()
    {
        return Ok(await _mediator.Send(new GetExchangeRateCommand()));
    }
}