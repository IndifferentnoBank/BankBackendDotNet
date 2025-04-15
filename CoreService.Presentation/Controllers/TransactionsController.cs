using Common.Helpers;
using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.CreateTransaction;
using CoreService.Application.Features.Commands.TransferMoney;
using CoreService.Application.Features.Queries.GetLoanPayments;
using CoreService.Application.Features.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
[Authorize(Policy = "CustomPolicy")]
[Route("bank_accounts")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id)
    {
        return Ok(await _mediator.Send(new GetTransactionsCommand(id, User.GetUserClaims())));
    }

    [HttpPost]
    [Route("{id:guid}/transactions")]
    public async Task<IActionResult> CreateTransaction(Guid id, CreateTransactionDto transaction)
    {
        return Ok(await _mediator.Send(new CreateTransactionCommand(id, User.GetUserClaims().UserId, transaction)));
    }

    [HttpPost]
    [Route("/transfer")]
    public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyDto transferMoneyDto)
    {
        return Ok(await _mediator.Send(new TransferMoneyCommand(User.GetUserClaims().UserId, transferMoneyDto)));
    }

    [HttpGet]
    [Route("/loan/{id:guid}/transactions")]
    public async Task<IActionResult> GetLoanTransactions(Guid id)
    {
        return Ok(await _mediator.Send(new GetLoanPaymentsCommand(User.GetUserClaims(), id)));
    }
}