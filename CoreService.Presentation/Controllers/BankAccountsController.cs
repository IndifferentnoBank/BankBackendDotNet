using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.CloseBankAccount;
using CoreService.Application.Features.Commands.CreateBankAccount;
using CoreService.Application.Features.Queries.GetBankAccountById;
using CoreService.Application.Features.Queries.GetBankAccounts;
using CoreService.Application.Features.Queries.GetBankAccountsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[Authorize]
[ApiController]
[Route("bank_accounts")]
public class BankAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBankAccounts(Guid userId, string? accountNumber = null, string? accountName = null)
    {
        return Ok(await _mediator.Send(new GetBankAccountsCommand(userId, accountNumber, accountName)));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetBankAccount(Guid id, Guid userId)
    {
        return Ok(await _mediator.Send(new GetBankAccountByIdCommand(id, userId)));
    }

    [HttpGet]
    [Route("{clientId:guid}/bank_accounts")]
    public async Task<IActionResult> GetBankAccounts(Guid userId, Guid clientId)
    {
        return Ok(await _mediator.Send(new GetBankAccountsByUserCommand(userId, clientId)));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> ClosBankAccount(Guid id, Guid userId)
    {
        return Ok(await _mediator.Send(new CloseBankAccountCommand(id, userId)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountDto accountDto, Guid userId)
    {
        return Ok(await _mediator.Send(new CreateBankAccountCommand(userId, accountDto)));
    }
}