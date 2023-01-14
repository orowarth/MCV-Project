﻿using MCBADataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MCBADataLibrary.Data;

public class BankDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BillPay> BillPays { get; set; }
    public DbSet<Payee> Payees { get; set; }
}
