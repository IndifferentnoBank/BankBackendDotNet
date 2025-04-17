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
[Route("core_service")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("bank_accounts/{id:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id)
    {
        return Ok(await _mediator.Send(new GetTransactionsCommand(id,
            JwtHelper.ExtractUserClaimsFromHeader(HttpContext))));
    }

    [HttpPost]
    [Route("bank_accounts/{id:guid}/transactions")]
    public async Task<IActionResult> CreateTransaction(Guid id, CreateTransactionDto transaction)
    {
        return Ok(await _mediator.Send(new CreateTransactionCommand(id,
            JwtHelper.ExtractUserClaimsFromHeader(HttpContext), transaction)));
    }

    [HttpPost]
    [Route("transfer")]
    public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyDto transferMoneyDto)
    {
        return Ok(await _mediator.Send(
            new TransferMoneyCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext), transferMoneyDto)));
    }

    [HttpGet]
    [Route("loan/{id:guid}/transactions")]
    public async Task<IActionResult> GetLoanTransactions(Guid id)
    {
        return Ok(await _mediator.Send(new GetLoanPaymentsCommand(JwtHelper.ExtractUserClaimsFromHeader(HttpContext),
            id)));
    }
}