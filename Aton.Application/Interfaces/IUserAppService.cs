using Aton.Application.ViewModels;

namespace Aton.Application.Interfaces;

public interface IUserAppService : IDisposable
{
    void Create(CreateUserViewModel createUserViewModel);
    IEnumerable<UserViewModel> GetAll();
    IEnumerable<UserViewModel> GetAll(int skip, int take);
    UserViewModel GetById(Guid id);
    void Update(UserViewModel userViewModel);
    void Remove(Guid id);
    IList<UserViewModel> GetAllHistory(Guid id);
}