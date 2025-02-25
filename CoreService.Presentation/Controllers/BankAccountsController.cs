using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.CloseBankAccount;
using CoreService.Application.Features.Commands.CreateBankAccount;
using CoreService.Application.Features.Queries.GetBankAccountById;
using CoreService.Application.Features.Queries.GetBankAccounts;
using CoreService.Application.Features.Queries.GetBankAccountsByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
public class BankAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("bank_accounts")]
    public async Task<IActionResult> GetBankAccounts(string? accountNumber = null, string? accountName = null)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");
        return Ok(await _mediator.Send(new GetBankAccountsCommand(userId, accountNumber, accountName)));
    }

    [HttpGet]
    [Route("bank_accounts/{id:guid}")]
    public async Task<IActionResult> GetBankAccount(Guid id)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");
        return Ok(await _mediator.Send(new GetBankAccountByIdCommand(id, userId)));
    }

    [HttpGet]
    [Route("users/{userId:guid}/bank_accounts")]
    public async Task<IActionResult> GetBankAccounts(Guid userId)
    {
        return Ok(await _mediator.Send(new GetBankAccountsByUserCommand(userId)));
    }

    [HttpDelete]
    [Route("bank_accounts/{id:guid}")]
    public async Task<IActionResult> ClosBankAccount(Guid id)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");;
        return Ok(await _mediator.Send(new CloseBankAccountCommand(id, userId)));
    }

    [HttpPost]
    [Route("bank_accounts")]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountDto accountDto)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");;
        return Ok(await _mediator.Send(new CreateBankAccountCommand(userId, accountDto)));
    }
}