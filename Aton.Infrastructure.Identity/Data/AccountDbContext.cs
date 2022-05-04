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

        modelBuilder
            .Entity<Account>()
            .HasData(
                new Account(
                    "Admin",
                    "10000.F7u53wgI2rVPR1dEvGHwcQ==.iMMgUCC7ELv9Bphqndc6kByHOq5aocfu2pS2UiMBZzI=",
                    true)
                {
                    Id = Guid.NewGuid()
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
        UpdateTimestamps(entities, user);
    }
    

    private void UpdateTimestamps(List<EntityEntry> entries, string user = null)
    {
        var filtered = entries
            .Where(x => x.State == EntityState.Added
                        || x.State == EntityState.Modified);
        

        foreach (var entry in filtered)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ((EntityAudit)entry.Entity).CreatedAt = DateTime.Now;
                    ((EntityAudit)entry.Entity).CreatedBy = user;
                    break;
                case EntityState.Modified:
                    ((EntityAudit)entry.Entity).UpdatedAt = DateTime.Now;
                    ((EntityAudit)entry.Entity).UpdatedBy = user;
                    break;
            }
        }
    }
}