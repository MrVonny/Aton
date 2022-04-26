using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Models;
using Aton.Infrastructure.Identity.Managers;
using Aton.Infrastructure.Identity.Models;
using Aton.Services.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aton.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UserController : ApiController
{
    private readonly IUserAppService _userAppService;
    private readonly AccountManager _accountManager;
    private readonly IUserAccountConnector _userAccountConnector;

    public UserController(
        IUserAppService userAppService,
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediator,
        AccountManager accountManager, IUserAccountConnector userAccountConnector) : base(notifications, mediator)
    {
        _userAppService = userAppService;
        _accountManager = accountManager;
        _userAccountConnector = userAccountConnector;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveOrdered()
    {
        var active = await _userAccountConnector.GetActiveOrdered();
        return Response(active);
    }
    
    [HttpGet]
    [Route("older-than/{olderThan:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOlderThan([FromRoute, Required] int? olderThan)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }   
        var users = await _userAccountConnector.GetOlderThan(olderThan!.Value);
        return Response(users);
    }

    [HttpGet]
    [Route("{login}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByLogin([FromRoute] string login)
    {
        if (!IsUserAdmin() && GetUserLogin() != login)
            return Forbid();

        var user = await _userAccountConnector.GetByLogin(login);
        if (user == null)
        {
            NotifyError(string.Empty, "Can't find user");
            return Response();
        }

        return Response(user);
    }

    [HttpGet]
    [Route("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Me()
    {
        var user = await _userAccountConnector.GetByLogin(GetUserLogin());
        
        if (user == null)
        {
            NotifyError(string.Empty,"Can't find user");
            return Response();
        }

        return Response(user);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response(createUserViewModel);
        }
        

        var account = new Account(
            createUserViewModel.Login,
            createUserViewModel.Password,
            createUserViewModel.Admin.GetValueOrDefault());
        
        _accountManager.CurrentUser = GetUserLogin();
        var identityResult = await _accountManager.CreateAsync(account);

        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
                return Response();
        }
        
        var userGuid = await _userAppService.Create(createUserViewModel, GetUserLogin());

        if (userGuid == null)
        {
            return Response();
        }

        _accountManager.CurrentUser = GetUserLogin();
        var mapResult = await _accountManager.MapToUser(createUserViewModel.Login, userGuid.Value);
        
        if (!mapResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }

        return Response(createUserViewModel);
    }
    
    [HttpPost]
    [Route("{login}/info")]
    public async Task<IActionResult> UpdateInfo([FromRoute(Name = "login")] string login,
        [FromQuery] string name = null,
        [FromQuery] Gender? gender = null,
        [FromQuery] DateTime? birthday = null)
    {
        if (!IsUserAdmin() && GetUserLogin() != login)
            return Forbid();

        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }
        
        var userGuid = await _accountManager.GetUserGuid(login);
        if (userGuid == null)
        {
           NotifyError(string.Empty, "User doesn't exists");
           return Response();
        }

        var model = new EditUserInfoModel(userGuid, name, gender, birthday);

        var user = await _userAppService.Edit(model, GetUserLogin());

        return Response(user);
    }

    [HttpPost]
    [Route("{login}/login")]
    public async Task<IActionResult> UpdateLogin([FromRoute(Name = "login")] string login, [FromQuery] string newLogin)
    {
        if (!IsUserAdmin() && GetUserLogin() != login)
            return Forbid();
        
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }

        _accountManager.CurrentUser = GetUserLogin();
        var identityResult = await _accountManager.ChangeLogin(login, newLogin);
        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }
        
        return Response();
    }
    
    [HttpPost]
    [Route("{login}/password")]
    public async Task<IActionResult> UpdatePassword([FromRoute(Name = "login")] string login, [FromQuery] string newPassword)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }

        _accountManager.CurrentUser = GetUserLogin();
        var identityResult =
            await _accountManager.ChangePassword(login, newPassword);
        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }
        
        return Response();
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("{login}/restore")]
    public async Task<IActionResult> Restore([FromRoute(Name = "login"), Required] string login)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }

        var restoreResult = await _accountManager.RestoreAsync(login);
        if (!restoreResult.IsSuccess)
        {
            AddIdentityErrors(restoreResult);
            return Response();
        }
        
        var userGuid = await _accountManager.GetUserGuid(login);
        if (userGuid == null)
        {
            NotifyError(string.Empty, "User doesn't exists");
            return Response();
        }

        await _userAppService.Restore(userGuid.Value);

        return Response();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{login}")]
    public async Task<IActionResult> Delete([FromRoute(Name = "login")] string login, [FromQuery] bool soft = true)
    {
        if (soft)
            await RevokeUser(login);
        else
            await DeleteUser(login);

        return Response();
    }

    private async Task RevokeUser(string login)
    {
        _userAccountConnector.CurrentUser = GetUserLogin();
        await _userAccountConnector.Revoke(login);
    }
    
    private async Task DeleteUser(string login)
    {
        _userAccountConnector.CurrentUser = GetUserLogin();
        await _userAccountConnector.Delete(login);
    }
    
    private string GetUserLogin()
    {
        return HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
    }
    
    private bool IsUserAdmin()
    {
        return HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
    }
}