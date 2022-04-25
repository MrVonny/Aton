using Aton.Domain.Intefaces;
using Aton.Domain.Models;
using Aton.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Data.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetActiveOrdered()
    {
        return await DbSet
            .Where(u => !u.RevokedAt.HasValue && !u.RevokedAt.HasValue)
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetOlderThan(int olderThan)
    {
        return await DbSet.Where(u =>
            u.Birthday.HasValue &&
            (u.Birthday.Value.AddYears(DateTime.Now.Year - u.Birthday.Value.Year) >
             DateTime.Now
                ? DateTime.Now.Year - u.Birthday.Value.Year - 1
                : DateTime.Now.Year - u.Birthday.Value.Year) > olderThan)
            .ToListAsync();
    }
}