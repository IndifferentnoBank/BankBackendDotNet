using System.Text;
using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CoreService.Application.Helpers.BankAccountNumberGenerator
{
    public class BankAccountNumberGenerator : IBankAccountNumberGenerator
    {
        private readonly int _accountNumberLength;

        public BankAccountNumberGenerator(IConfiguration configuration)
        {
            _accountNumberLength = int.Parse(configuration["BankAccountNumberLength"]!);
        }

        public string GenerateBankAccountNumber()
        {
            var random = new Random();
            var accountNumber = new StringBuilder();

            accountNumber.Append(random.Next(1, 10)); 

            for (var i = 1; i < _accountNumberLength; i++)
            {
                accountNumber.Append(random.Next(0, 10)); 
            }

            return accountNumber.ToString();
        }
    }
}