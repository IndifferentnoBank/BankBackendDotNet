using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.CloseBankAccount;
using CoreService.Application.Features.Commands.CreateBankAccount;
using CoreService.Application.Features.Queries.GetBankAccountById;
using CoreService.Application.Features.Queries.GetBankAccounts;
using CoreService.Application.Features.Queries.GetBankAccountsByPhoneNumber;
using CoreService.Application.Features.Queries.GetBankAccountsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
[Route("core_service/bank_accounts")]
public class BankAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetBankAccounts(string? accountNumber = null, string? accountName = null)
    {
        return Ok(await _mediator.Send(new GetBankAccountsCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext),
            accountNumber, accountName)));
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetBankAccount(Guid id)
    {
        return Ok(await _mediator.Send(new GetBankAccountByIdCommand(id,
            JwtHelper.ExtractUserClaimsFromHeader(HttpContext))));
    }

    [HttpGet]
    [Route("{clientId:guid}/bank_accounts")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetBankAccounts(Guid clientId)
    {
        return Ok(await _mediator.Send(
            new GetBankAccountsByUserCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext), clientId)));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> ClosBankAccount(Guid id)
    {
        return Ok(await _mediator.Send(new CloseBankAccountCommand(id,
            JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId)));
    }

    [HttpPost]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountDto accountDto)
    {
        return Ok(await _mediator.Send(
            new CreateBankAccountCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext), accountDto)));
    }
    
    [HttpGet]
    [Route("phone_number")]
    public async Task<IActionResult> GetBankAccountsByPhoneNumber([FromQuery]string phoneNumber)
    {
        return Ok(await _mediator.Send(new GetBankAccountsByPhoneNumberCommand(phoneNumber)));
    }
}