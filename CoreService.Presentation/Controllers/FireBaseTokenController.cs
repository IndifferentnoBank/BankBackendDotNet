using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.SendToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
[Authorize(Policy = "CustomPolicy")]
[Route("core_service/firebase/token")]
public class FireBaseTokenController : ControllerBase
{
    private readonly ISender _sender;

    public FireBaseTokenController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> SaveToken([FromBody] FireBaseTokenDto fireBaseTokenDto)
    {
        return Ok(await _sender.Send(new SendTokenCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext),
            fireBaseTokenDto.Service,
            fireBaseTokenDto.Token)));
    }
}