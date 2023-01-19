using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;
using MCBAWebApp.Converters;
using System.Text.Json;

namespace MCBAWebApp.Services;

public class WebDataAccess
{
    public static async Task InitalizeData(IServiceProvider serviceProvider)
    {
        const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        using var context = serviceProvider.GetRequiredService<BankDbContext>();

        if (context.Customers.Any())
            return;

        using var client = new HttpClient();

        var jsonData = await client.GetStringAsync(Url);

        var jsonOptions = new JsonSerializerOptions()
        {
            Converters =
            {
                new DateTimeFormatConverter(),
                new AccountTypeConverter()
            }
        };

        var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData, jsonOptions)!;

        foreach (var customer in customers)
        {
            foreach (var account in customer.Accounts)
            {
                account.Balance = account.Transactions.Sum(t => t.Amount);
            }
        }

        await context.AddRangeAsync(customers);
        await context.AddRangeAsync(GetSeedPayees());
        await context.SaveChangesAsync();
    }

    public static ICollection<Payee> GetSeedPayees()
    {
        return new List<Payee>()
        {
            new Payee
            {
                Name = "Telco Co.",
                Address = "1 City Street",
                City = "Melbourne",
                PostCode= "3000",
                Phone = "(04) 0000 0000",
                State = State.VIC
            },

            new Payee
            {
                Name = "Insurance Company",
                Address = "33 Business Road",
                City = "Sydney",
                PostCode = "2000",
                Phone = "(04) 2222 2222",
                State = State.NSW
            },

            new Payee
            {
                Name = "Leaky Loans",
                Address = "27 Beach Lane",
                City = "Perth",
                PostCode = "6000",
                Phone = "(04) 1212 3434",
                State = State.WA
            },

            new Payee
            {
                Name = "Useless Utilities",
                Address = "277 Bridge Street",
                City = "Hobart",
                PostCode = "4000",
                Phone = "(04) 8765 4432",
                State = State.QLD
            },
        };
    }
}
