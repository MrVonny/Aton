using Aton.Domain.Models;

namespace Aton.Domain.Intefaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetActiveOrdered();
        Task<IEnumerable<User>> GetOlderThan(int olderThan);
    }
}
