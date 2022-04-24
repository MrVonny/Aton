using System.Security.Claims;
using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Models;
using Aton.Infrastructure.Identity.Managers;
using Aton.Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

namespace Aton.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UserController : ApiController
{
    private readonly IUserAppService _userAppService;
    private readonly AccountManager _accountManager;
    private readonly SignInManager _signInManager;

    public UserController(
        IUserAppService userAppService,
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediator,
        AccountManager accountManager,
        SignInManager signInManager) : base(notifications, mediator)
    {
        _userAppService = userAppService;
        _accountManager = accountManager;
        _signInManager = signInManager;
        _accountManager.CurrentUser = GetUserLogin();
    }

    [HttpGet]
    [Route("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveOrdered()
    {
        var active = _userAppService.GetActiveOrdered();
        return Response(active);
    }
    
    [HttpGet]
    [Route("older-than/{olderThan:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOlderThan([FromRoute] int? olderThan)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{login}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByLogin([FromRoute] string login)
    {
        if (IsUserAdmin() || GetUserLogin() == login)
        {
            var guid =
                await _accountManager.GetUserGuid(login);
            if (guid == null)
            {
                NotifyError(string.Empty,"Can't find user");
                return Response();
            }

            var user = await _userAppService.GetById(guid.Value);
        
            return Response(user);
        }
        NotifyError("","Forbidden");
        return Response();
    }
    
    [HttpGet]
    [Route("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Me()
    {
        var login = GetUserLogin();
        return await GetByLogin(login);
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
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }

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

        var identityResult =
            await _accountManager.ChangePassword(login, newPassword);
        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }
        
        return Response();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{login}")]
    public async Task<IActionResult> Delete([FromRoute(Name = "login")] string login, [FromQuery] bool soft = true)
    {
        var guid = await _accountManager.GetUserGuid(login);
        if (guid == null)
        {
            NotifyError(string.Empty,"Can't find user");
            return Response();
        }

        if (soft)
            await RevokeUser(login, guid.Value);
        else
            await DeleteUser(login, guid.Value);

        return Response();
    }

    private async Task RevokeUser(string login, Guid guid)
    {
        var result = await _accountManager.RevokeAsync(login);
        if (!result.IsSuccess)
        {
            AddIdentityErrors(result);
            return;
        }
        await _userAppService.Revoke(guid, GetUserLogin());
    }
    
    private async Task DeleteUser(string login, Guid guid)
    {
        var result = await _accountManager.Remove(login);
        if (!result.IsSuccess)
        {
            AddIdentityErrors(result);
            return;
        }
        _userAppService.Remove(guid);
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