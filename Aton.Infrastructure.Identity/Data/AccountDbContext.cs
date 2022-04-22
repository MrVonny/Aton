using Aton.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Data;

public sealed class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Account> Accounts { get; set; }
}