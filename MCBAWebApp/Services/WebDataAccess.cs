using MCBADataLibrary.Data;
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
        await context.SaveChangesAsync();
    }
}
