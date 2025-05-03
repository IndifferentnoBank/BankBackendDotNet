using System.Globalization;
using CoreService.Contracts.Interfaces;
using CoreService.Contracts.Repositories;
using CoreService.Domain.Entities;
using FirebaseAdmin.Messaging;

namespace CoreService.Infrastructure.FireBase;

public class FireBaseSender : IFireBaseSender
{
    private readonly IFireBaseTokenRepository _fireBaseTokenRepository;

    public FireBaseSender(IFireBaseTokenRepository fireBaseTokenRepository)
    {
        _fireBaseTokenRepository = fireBaseTokenRepository;
    }

    public async Task SendMessageToClientAsync(Guid userId, Transaction transaction)
    {
        var tokens = await _fireBaseTokenRepository.GetFireBaseTokenByUserIdAsync(userId);

        var message = GetMessage(tokens, transaction);

        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

        await CleanTokens(response, tokens, _fireBaseTokenRepository);
    }

    public async Task SendMessageToStaffAsync(Transaction transaction)
    {
        var tokens = await _fireBaseTokenRepository.GetFireBaseTokenByRoleAsync();

        var message = GetMessage(tokens, transaction);

        var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

        await CleanTokens(response, tokens, _fireBaseTokenRepository);
    }

    private MulticastMessage GetMessage(IReadOnlyList<string> tokens, Transaction transaction)
    {
        return new MulticastMessage()
        {
            Tokens = tokens,
            Data = new Dictionary<string, string?>()
            {
                { "Type", transaction.Type.ToString() },
                { "Amount", transaction.Amount.ToString("F2", CultureInfo.InvariantCulture) },
                { "Currency", transaction.Currency.ToString() },
                { "Comment", transaction.Comment ?? string.Empty },
                { "Time", transaction.Date.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) },
            },
        };
    }

    private static async Task CleanTokens(BatchResponse response, IReadOnlyList<string> tokens,
        IFireBaseTokenRepository fireBaseTokenRepository)
    {
        if (response.FailureCount > 0)
        {
            var failedTokens = new List<string>();
            for (var i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                {
                    failedTokens.Add(tokens[i]);
                }
            }

            await fireBaseTokenRepository.DeleteRangeOfTokensAsync(failedTokens);
        }
    }
}