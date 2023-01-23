using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;

namespace MCBAWebApp.Tests;

public static class SeedData
{
    public static void Initialize(BankDbContext context)
    {
        var customers = new List<Customer>()
        {
            new Customer
            {
                CustomerID = 2100,
                Name = "Test",
                TFN = null,
                Address = "123 Test Lane",
                City = "TestCity",
                State = null,
                PostCode = "3000",
                Mobile = null,
                CustomerStatus = CustomerStatus.Unblocked,
                Accounts = new List<Account>()
                {
                    new Account(AccountType.Savings)
                    {
                        AccountNumber = 4100,
                        Balance = 100,
                        CustomerID = 2100,
                        Transactions = new List<Transaction>()
                        {
                            new Transaction()
                            {
                                TransactionType = TransactionType.Deposit,
                                AccountNumber = 4100,
                                DestinationAccountNumber = null,
                                Amount = 100,
                                Comment = "Opening",
                                TransactionTimeUtc = DateTime.UtcNow
                            }
                        }
                    },

                },
                Login = new Login
                {
                    LoginID = "12345678",
                    PasswordHash = "Rfc2898DeriveBytes$50000$MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE="
                }
            },

        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}
