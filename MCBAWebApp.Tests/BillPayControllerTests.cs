using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Controllers;
using MCBAWebApp.Models;
using MCBAWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MCBAWebApp.Tests;

public class BillPayControllerTests
{
  private readonly BankDbContext _context;

  public BillPayControllerTests()
  {
    _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
        UseInMemoryDatabase("BillPayDb").Options);
    SeedData.Initialize(_context);
  }

  public void Dispose()
  {
    _context.Database.EnsureDeleted();
    _context.Dispose();
  }

//   [Fact]
//   public async Task BillPayService_WhenCanelled_Cancels()
//   {
//     var serviceCollection = new ServiceCollection();

//     serviceCollection.Add(_context);

//     var cts = new CancellationTokenSource();
//     var task = new BillPayService(serviceCollection);
//     var taskResult = task.ExecuteAsync(cts.Token);

//     await Task.Delay(1000);
//     cts.Cancel();

//     await taskResult.ContinueWith(t =>
//     {
//       Assert.True(t.IsCanceled);
//     });
//   }



}