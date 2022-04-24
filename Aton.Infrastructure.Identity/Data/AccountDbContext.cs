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
            .WithOne(a => a.AccountToUser)
            .HasForeignKey<AccountToUser>(atu => atu.AccountId);

        modelBuilder.Entity<AccountToUser>()
            .HasOne(atu => atu.User)
            .WithOne()
            .HasForeignKey<AccountToUser>(atu => atu.UserId);

        var acc1 = new Account(
            "Kitty17",
            "hQsdf7sh", true)
        {
            Guid = Guid.NewGuid()
        };
        var acc2 = new Account(
            "Vasya1999",
            "hQsdf7sh")
        {
            Guid = Guid.NewGuid()
        };

        modelBuilder
            .Entity<Account>()
            .HasData(
                new Account(
                    "Admin",
                    "Admin123",
                    true)
                {
                    Guid = Guid.NewGuid()
                },
                acc1,
                acc2);

        modelBuilder.Entity<AccountToUser>()
            .HasData(
                new AccountToUser()
                {
                    AccountId = acc1.Guid,
                    UserId = Guid.Parse("c6a774c5-8729-454e-b1dc-348a0f220795"),
                    Guid = Guid.NewGuid()
                },
                new AccountToUser()
                {
                    AccountId = acc2.Guid,
                    UserId = Guid.Parse("7940d819-483c-4d27-929a-2879d41c0dad"),
                    Guid = Guid.NewGuid()
                });
    }

}