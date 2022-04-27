using System.Collections.Concurrent;
using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Infrastructure.Identity.Managers;
using Aton.Services.Api.ViewModels;
using AutoMapper;

namespace Aton.Services.Api.Services;

public class UserAccountConnector : IUserAccountConnector
{
    private readonly IUserAppService _userAppService;
    private readonly AccountManager _accountManager;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    public string CurrentUser
    {
        get => _accountManager.CurrentUser;
        set => _accountManager.CurrentUser = value;
    }

    public UserAccountConnector(AccountManager accountManager,
        IUserAppService userAppService,
        IMapper mapper,
        IMediatorHandler mediator)
    {
        _accountManager = accountManager;
        _userAppService = userAppService;
        _mapper = mapper;
        _mediator = mediator;
    }
    
    private void NotifyError(string code, string message)
    {
        _mediator.RaiseEvent(new DomainNotification(code, message));
    }

    private void AddIdentityErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            NotifyError(result.ToString(), error.Description);
        }
    }

    private async Task<AspUserViewModel> GetAspViewModel(UserViewModel model)
    {
        var aspViewModel = _mapper.Map<AspUserViewModel>(model);
        aspViewModel.Login = await _accountManager.GetUserLogin(model.Guid);
        return aspViewModel;
    }
    
    private async Task<IEnumerable<AspUserViewModel>> GetAspViewModels(IEnumerable<UserViewModel> models)
    {
        var list = new ConcurrentBag<AspUserViewModel>();
        foreach (var model in models)
        {
            list.Add(await GetAspViewModel(model));
        }
        return list;
    }


    public async Task<IEnumerable<AspUserViewModel>> GetActiveOrdered()
    {
        var active = await _userAppService.GetActiveOrdered();
        return await GetAspViewModels(active);
    }

    public async Task<IEnumerable<AspUserViewModel>> GetOlderThan(int olderThan)
    {
        var users = await _userAppService.GetOlderThan(olderThan);
        return await GetAspViewModels(users);
    }

    public async Task<AspUserViewModel>  GetByLogin(string login)
    {
        var guid = await _accountManager.GetUserGuid(login);
        if (guid == null)
        {
            NotifyError(string.Empty, "User doesn't exists");
            return null;
        }
        var user = await _userAppService.GetById(guid.Value);
        return await GetAspViewModel(user);
    }

    public async Task Delete(string login)
    {
        var guid = await _accountManager.GetUserGuid(login);
        if (guid == null)
        {
            NotifyError(string.Empty,"User doesn't exists");
            return;
        }
        
        var result = await _accountManager.Remove(login);
        if (!result.IsSuccess)
        {
            AddIdentityErrors(result);
            return;
        }
        _userAppService.Remove(guid.Value);
    }

    public async Task Revoke(string login)
    {
        var guid = await _accountManager.GetUserGuid(login);
        if (guid == null)
        {
            NotifyError(string.Empty,"User doesn't exists");
            return;
        }   
        var result = await _accountManager.RevokeAsync(login);
        if (!result.IsSuccess)
        {
            AddIdentityErrors(result);
            return;
        }
        await _userAppService.Revoke(guid.Value, CurrentUser);
    }
}

public interface IUserAccountConnector
{
    public string CurrentUser { get; set; }
    public Task<IEnumerable<AspUserViewModel>> GetActiveOrdered();
    public Task<IEnumerable<AspUserViewModel>> GetOlderThan(int olderThan);
    public Task<AspUserViewModel> GetByLogin(string login);
    public Task Delete(string login);
    public Task Revoke(string login);
}