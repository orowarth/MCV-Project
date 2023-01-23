using MCBAAdminAPI.Data;
using MCBADataLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BankDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BankContext"));
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBillPayRepository, BillPayRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * This is removed as the 'dotnet run' command does not expose https://localhost:3000,
 * so there's no real point in using https redirection anyways.
 * 
 * app.UseHttpsRedirection();
 */
app.UseAuthorization();

app.MapControllers();

app.Run();
