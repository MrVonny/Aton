using Aton.Domain.Core.Models;
using Aton.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aton.Infrastructure.Data.Contexts;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>();
        // .HasData(
        //     new User(
        //         Guid.Parse("c6a774c5-8729-454e-b1dc-348a0f220795"),
        //         "Молева Екатерина",
        //         Gender.Male,
        //         DateTime.Parse("1992-04-14")),
        //     new User(
        //         Guid.Parse("7940d819-483c-4d27-929a-2879d41c0dad"),
        //         "Козлов Кирилл",
        //         Gender.Male,
        //         DateTime.Parse("1992-04-14"))
        // );
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is EntityAudit)
            .ToList();
        //UpdateSoftDelete(entities);
        UpdateTimestamps(entities);
    }

    private void UpdateSoftDelete(List<EntityEntry> entries)
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

    private void UpdateTimestamps(List<EntityEntry> entries)
    {
        var filtered = entries
            .Where(x => x.State == EntityState.Added
                        || x.State == EntityState.Modified);
        

        foreach (var entry in filtered)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ((EntityAudit)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((EntityAudit)entry.Entity).UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    ((EntityAudit)entry.Entity).UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}