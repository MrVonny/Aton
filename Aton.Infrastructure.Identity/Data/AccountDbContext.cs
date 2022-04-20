using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Data;

public sealed class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
        
    }
    
}