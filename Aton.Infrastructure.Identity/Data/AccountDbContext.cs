using Aton.Domain.Core.Models;
using Aton.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            Id = Guid.NewGuid()
        };
        var acc2 = new Account(
            "Vasya1999",
            "hQsdf7sh")
        {
            Id = Guid.NewGuid()
        };

        modelBuilder
            .Entity<Account>()
            .HasData(
                new Account(
                    "Admin",
                    "Admin123",
                    true)
                {
                    Id = Guid.NewGuid()
                },
                acc1,
                acc2);

        modelBuilder.Entity<AccountToUser>()
            .HasData(
                new AccountToUser()
                {
                    AccountId = acc1.Id,
                    UserId = Guid.Parse("c6a774c5-8729-454e-b1dc-348a0f220795"),
                    Guid = Guid.NewGuid()
                },
                new AccountToUser()
                {
                    AccountId = acc2.Id,
                    UserId = Guid.Parse("7940d819-483c-4d27-929a-2879d41c0dad"),
                    Guid = Guid.NewGuid()
                });
    }
    
    public async Task<int> SaveChangesWithUserAsync(string user = null)
    {
        OnBeforeSaving(user);
        return await SaveChangesAsync();
    }

    private void OnBeforeSaving(string user = null)
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is EntityAudit)
            .ToList();
        //UpdateSoftDelete(entities, user);
        UpdateTimestamps(entities, user);
    }

    private void UpdateSoftDelete(List<EntityEntry> entries, string user = null)
    {
        var filtered = entries
            .Where(x => x.State == EntityState.Added
                        || x.State == EntityState.Deleted);

        foreach (var entry in filtered)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    //entry.CurrentValues["IsDeleted"] = false;
                    ((EntityAudit)entry.Entity).IsDeleted = false;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    //entry.CurrentValues["IsDeleted"] = true;
                    ((EntityAudit)entry.Entity).IsDeleted = true;
                    break;
            }
        }
    }

    private void UpdateTimestamps(List<EntityEntry> entries, string user = null)
    {
        var filtered = entries
            .Where(x => x.State == EntityState.Added
                        || x.State == EntityState.Modified);
        

        foreach (var entry in filtered)
        {
            if (entry.State == EntityState.Added)
            {
                ((EntityAudit)entry.Entity).CreatedAt = DateTime.UtcNow;
                ((EntityAudit)entry.Entity).CreatedBy = user;
            }

            ((EntityAudit)entry.Entity).UpdatedAt = DateTime.UtcNow;
            ((EntityAudit)entry.Entity).UpdatedBy = user;
        }
    }
}