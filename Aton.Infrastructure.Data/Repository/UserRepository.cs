using Aton.Domain.Intefaces;
using Aton.Domain.Models;
using Aton.Infrastructure.Data.Contexts;

namespace Aton.Infrastructure.Data.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public User GetByLogin(string login)
    {
        throw new NotImplementedException();
    }
}