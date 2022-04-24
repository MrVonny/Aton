using Aton.Domain.Models;

namespace Aton.Domain.Intefaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetActiveOrdered();
    }
}
