using CoreService.Application.Dtos.Requests;
using CoreService.Application.Features.Commands.CreateTransaction;
using CoreService.Application.Features.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.Presentation.Controllers;

[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("bank_accounts/{id:guid}transactions")]
    public async Task<IActionResult> GetTransactions(Guid id)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");;
        return Ok(await _mediator.Send(new GetTransactionsCommand(id, userId)));
    }

    [HttpPost]
    [Route("bank_accounts/{id:guid}/transactions")]
    public async Task<IActionResult> CreateTransaction(Guid id, CreateTransactionDto transaction)
    {
        var userId = Guid.Parse("741eb8f8-fe51-4d24-ba62-9a133bc61893");
        return Ok(await _mediator.Send(new CreateTransactionCommand(id, userId, transaction)));
    }
}