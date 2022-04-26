using Aton.Application.ViewModels;

namespace Aton.Application.Interfaces;

public interface IUserAppService : IDisposable
{
    Task<Guid?> Create(CreateUserViewModel createUserViewModel, string createdBy);
    IEnumerable<UserViewModel> GetAll();
    Task<IEnumerable<UserViewModel>> GetActiveOrdered();
    IEnumerable<UserViewModel> GetAll(int skip, int take);
    Task<UserViewModel> Edit(EditUserInfoModel editUserInfoModel, string updatedBy);
    Task<UserViewModel> GetById(Guid id);
    void Update(UserViewModel userViewModel);
    void Remove(Guid id);
    IList<UserViewModel> GetAllHistory(Guid id);
    Task Revoke(Guid guid, string revokedBy);
    Task<IEnumerable<UserViewModel>> GetOlderThan(int olderThan);
    Task Restore(Guid guid);
}