using Aton.Domain.Models;

namespace Aton.Domain.Intefaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByLogin(string login);
        IEnumerable<User> GetActiveOrdered();
    }
}
