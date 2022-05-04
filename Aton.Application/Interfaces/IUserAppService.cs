using Aton.Application.ViewModels;

namespace Aton.Application.Interfaces;

public interface IUserAppService : IDisposable
{
    Task<Guid?> Create(CreateUserViewModel createUserViewModel, string createdBy);
    Task<IEnumerable<UserViewModel>> GetActiveOrdered();
    Task<UserViewModel> Edit(EditUserInfoModel editUserInfoModel, string updatedBy);
    Task<UserViewModel> GetById(Guid id);
    Task Remove(Guid id);
    Task Revoke(Guid guid, string revokedBy);
    Task<IEnumerable<UserViewModel>> GetOlderThan(int olderThan);
    Task Restore(Guid guid);
}