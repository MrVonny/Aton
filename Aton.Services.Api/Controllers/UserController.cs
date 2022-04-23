using Aton.Application.Interfaces;
using Aton.Application.ViewModels;
using Aton.Domain.Core.Bus;
using Aton.Domain.Core.Notifications;
using Aton.Infrastructure.Identity.Managers;
using Aton.Infrastructure.Identity.Models;
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
    }

    [HttpGet]
    [Route("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveOrdered()
    {
        var active = _userAppService.GetActiveOrdered();
        return Response(active);
    }

    #region Create
    
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
        var userGuid = await _userAppService.Create(createUserViewModel);

        var account = new Account(
            createUserViewModel.Login,
            createUserViewModel.Password,
            userGuid,
            createUserViewModel.Admin.GetValueOrDefault());

        var identityResult = await _accountManager.CreateAsync(account);

        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
                return Response();
        }
        
        return Response(createUserViewModel);
    }

    #endregion

    // #region Update-1
    //
    [HttpPost]
    [Route("{login}/info")]
    public async Task<IActionResult> Update(EditUserInfoModel editUserInfoModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response(editUserInfoModel);
        }

        var guid = await _accountManager.GetGuid(editUserInfoModel.Login);
        if (guid == null)
        {
           NotifyError(string.Empty, "User doesn't exists");
           return Response(editUserInfoModel);
        }

        var user = await _userAppService.Edit(editUserInfoModel);

        return Response(user);
    }

    [HttpPost]
    [Route("{login}/login")]
    public async Task<IActionResult> Update(EditAccountLoginModel editUserLoginModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response(editUserLoginModel);
        }

        var identityResult = await _accountManager.ChangeLogin(editUserLoginModel.Login, editUserLoginModel.NewLogin);
        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }
        
        return Response(editUserLoginModel);
    }
    
    [HttpPost]
    [Route("{login}/password")]
    public async Task<IActionResult> Update(EditAccountPasswordModel editUserPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response(editUserPasswordModel);
        }

        var identityResult =
            await _accountManager.ChangePassword(editUserPasswordModel.Login, editUserPasswordModel.NewPassword);
        if (!identityResult.IsSuccess)
        {
            AddIdentityErrors(identityResult);
            return Response();
        }
        
        return Response(editUserPasswordModel);
    }
    //
    // #endregion
    //
    // #region Read
    //
    //
    // /// <summary>
    // /// 5) Запрос списка всех активных (отсутствует RevokedOn) пользователей, список отсортирован по
    // /// CreatedOn (Доступно Админам)
    // /// </summary>
    // /// <returns></returns>
    // [HttpGet]
    // [Route("active")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult> GetActive()
    // {
    //     try
    //     {
    //         var users = await _context.Users
    //             .Where(u => u.RevokedOn.HasValue)
    //             .OrderBy(u => u.CreatedOn)
    //             .ToIndexModel()
    //             .ToListAsync();
    //
    //         return Ok(users);
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError);
    //     }
    // }
    //
    // [HttpGet]
    // [Route("{login}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult> GetUser([FromRoute(Name = "login")] string login)
    // {
    //     var user = await _context.Users.SingleOrDefaultAsync(u => u.Login.Equals(login));
    //     if (user != null)
    //         return Ok(user.ToIndexModel());
    //     return BadRequest("No such user");
    // }
    //
    // /// <summary>
    // /// 7) Запрос пользователя по логину и паролю (Доступно только самому пользователю, если он
    // /// активен (отсутствует RevokedOn))
    // /// </summary>
    // /// <returns></returns>
    // /// <exception cref="NotImplementedException"></exception>
    // [HttpGet]
    // [Route("me")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult> Me()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // /// <summary>
    // /// 8) Запрос всех пользователей старше определённого возраста (Доступно Админам)
    // /// </summary>
    // /// <returns></returns>
    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult> GetOlderThan([FromQuery] int? olderThan)
    // {
    //     if (olderThan is null or <= 0 or > 100)
    //         return BadRequest("Invalid age");
    //     try
    //     {
    //         var users = await _context.Users
    //             .OlderThan(olderThan.Value)
    //             .ToIndexModel()
    //             .ToListAsync();
    //
    //         return Ok(users);
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError);
    //     }
    // }
    //
    // #endregion

}