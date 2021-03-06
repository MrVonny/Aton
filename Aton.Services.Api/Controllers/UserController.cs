using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;
using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Domain.Models;
using Aton.Infrastructure.Identity.Managers;
using Aton.Services.Api.Services;
using Aton.Services.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [ProducesResponseType(typeof(List<AspUserViewModel>),StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Returns a list of active users sorted by creation date",
        Description = "Requires admin privileges"
    )]
    public async Task<IActionResult> GetActiveOrdered()
    {
        var active = await _userAccountConnector.GetActiveOrdered();
        return Response(active);
    }
    
    [HttpGet]
    [Route("older-than/{olderThan:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<AspUserViewModel>),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Returns a list of users older than a certain age",
        Description = "Requires admin privileges"
    )]
    public async Task<IActionResult> GetOlderThan([FromRoute, Required, Range(0, 100), SwaggerParameter("Age")] int? olderThan)
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
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Returns the user with the specified login",
        Description = "Requires admin rights only to get other users"
    )]
    public async Task<IActionResult> GetByLogin([FromRoute, SwaggerParameter("User's login")] string login)
    {
        if (!IsUserAdmin() && GetUserLogin() != login)
            return Forbid();

        var user = await _userAccountConnector.GetByLogin(login);
        if (user == null)
        {
            return Response();
        }

        return Response(user);
    }

    [HttpGet]
    [Route("me")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [SwaggerOperation(
        Summary = "Returns the user who sent the request",
        Description = "No admin rights required"
    )]
    public async Task<IActionResult> Me()
    {
        var user = await _userAccountConnector.GetByLogin(GetUserLogin());
        
        if (user == null)
        {
            return Response();
        }

        return Response(user);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Creates new user",
        Description = "Requires admin privileges"
    )]
    public async Task<IActionResult> Create(AspCreateUserViewModel aspCreateUserViewModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response(aspCreateUserViewModel);
        }

        _userAccountConnector.CurrentUser = GetUserLogin();

        var created = await _userAccountConnector.CreateUser(aspCreateUserViewModel);
        if (created == null)
            return Response();

        return CreatedResponse($"api/v1/users/{created.Login}", created);
    }
    
    [HttpPost]
    [Route("{login}/info")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Changes user information",
        Description = "No admin rights required to edit yourself. Requires admin rights only to edit other users"
    )]
    public async Task<IActionResult> UpdateInfo([FromRoute(Name = "login"), SwaggerParameter("User's login")] string login,
        [FromQuery, SwaggerParameter("User's name")] string name = null,
        [FromQuery, SwaggerParameter("User's gender")] Gender? gender = null,
        [FromQuery, SwaggerParameter("User's Date of Birth")] DateTime? birthday = null)
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

        await _userAppService.Edit(model, GetUserLogin());

        return Response(await _userAccountConnector.GetByLogin(login));
    }

    [HttpPost]
    [Route("{login}/login")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Changes user's login",
        Description = "No admin rights required to edit yourself. Requires admin rights only to edit other users"
    )]
    public async Task<IActionResult> UpdateLogin([FromRoute(Name = "login"), SwaggerParameter("User's login")] string login,
        [FromQuery, SwaggerParameter("New login")] string newLogin)
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
        
        return Response(await _userAccountConnector.GetByLogin(newLogin));
    }
    
    [HttpPost]
    [Route("{login}/password")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Changes user's password",
        Description = "No admin rights required to edit yourself. Requires admin rights only to edit other users"
    )]
    public async Task<IActionResult> UpdatePassword([FromRoute(Name = "login"), SwaggerParameter("User's login")] string login,
        [FromQuery, SwaggerParameter("New password")] string newPassword)
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
        
        return Response(await _userAccountConnector.GetByLogin(login));
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("{login}/restore")]
    [ProducesResponseType(typeof(AspUserViewModel),StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Restores a deleted user",
        Description = "Requires admin privileges"
    )]
    public async Task<IActionResult> Restore([FromRoute(Name = "login"), SwaggerParameter("User's login"), Required] string login)
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

        return Response(_userAccountConnector.GetByLogin(login));
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{login}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Deletes a user",
        Description = "Requires admin privileges"
    )]
    public async Task<IActionResult> Delete([FromRoute(Name = "login"), SwaggerParameter("User's login")] string login,
        [FromQuery, SwaggerParameter("Set to true to be able to restore the user")] bool soft = true)
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