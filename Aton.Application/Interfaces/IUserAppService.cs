using Aton.Application.ViewModels;

namespace Aton.Application.Interfaces;

public interface IUserAppService : IDisposable
{
    Task<Guid> Create(CreateUserViewModel createUserViewModel);
    IEnumerable<UserViewModel> GetAll();
    IEnumerable<UserViewModel> GetActiveOrdered();
    IEnumerable<UserViewModel> GetAll(int skip, int take);
    Task<UserViewModel> Edit(EditUserInfoModel editUserInfoModel);
    UserViewModel GetById(Guid id);
    void Update(UserViewModel userViewModel);
    void Remove(Guid id);
    IList<UserViewModel> GetAllHistory(Guid id);
}