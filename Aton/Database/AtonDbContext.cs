using Aton.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aton.Database;

public sealed class AtonDbContext : IdentityDbContext<User>
{
    public AtonDbContext(DbContextOptions<AtonDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
}