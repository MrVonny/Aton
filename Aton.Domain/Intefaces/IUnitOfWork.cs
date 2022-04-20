namespace Aton.Domain.Intefaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}
