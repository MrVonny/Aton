using Aton.Domain.Models;
using Aton.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Data;

public sealed class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountToUser> AccountToUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountToUser>()
            .HasOne(atu => atu.Account)
            .WithOne(a=>a.AccountToUser)
            .HasForeignKey<AccountToUser>(atu=>atu.AccountLogin);
        
        modelBuilder.Entity<AccountToUser>()
            .HasOne(atu => atu.User)
            .WithOne()
            .HasForeignKey<AccountToUser>(atu=>atu.UserId);
        
        modelBuilder
            .Entity<Account>()
            .HasData(new Account("Admin", "Admin123", true));
    }
    
}