using Aton.Database;
using Aton.Models.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aton.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly AtonDbContext _context;

    public UserController(AtonDbContext context)
    {
        _context = context;
    }

    #region Create

    /// <summary>
    /// 1) Создание пользователя по логину, паролю, имени, полу и дате рождения + указание будет ли
    /// пользователь админом (Доступно Админам)
    /// </summary>
    /// <param name="createUserModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(CreateUserModel createUserModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CreateUserModel, User>());
            var mapper = new Mapper(config);

            var user = mapper.Map<CreateUserModel, User>(createUserModel);

            if (await _context.Users.AnyAsync(u => u.Login == createUserModel.Login))
                _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user.ToIndexModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion

    #region Update-1

    [HttpPost]
    [Route("{login}/info")]
    public async Task<ActionResult> Update(EditUserInfoModel editUserInfoModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EditUserInfoModel, User>());
            var mapper = new Mapper(config);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(editUserInfoModel.Id));

            if (user == null)
                return BadRequest($"User with ID {editUserInfoModel.Id} doesn't exists");

            mapper.Map(editUserInfoModel, user);

            await _context.SaveChangesAsync();
            return Ok(user.ToIndexModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{login}/login")]
    public async Task<ActionResult> Update(EditUserLoginModel editUserLoginModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EditUserLoginModel, User>());
            var mapper = new Mapper(config);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(editUserLoginModel.Login));

            if (user == null)
                return BadRequest($"User with login {editUserLoginModel.Login} doesn't exists");

            mapper.Map(editUserLoginModel, user);

            await _context.SaveChangesAsync();
            return Ok(user.ToIndexModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{login}/password")]
    public async Task<ActionResult> Update(EditUserPasswordModel editUserPasswordModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EditUserPasswordModel, User>());
            var mapper = new Mapper(config);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(editUserPasswordModel.Login));

            if (user == null)
                return BadRequest($"User with login {editUserPasswordModel.Login} doesn't exists");

            mapper.Map(editUserPasswordModel, user);

            await _context.SaveChangesAsync();
            return Ok(user.ToIndexModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion

    #region Read


    /// <summary>
    /// 5) Запрос списка всех активных (отсутствует RevokedOn) пользователей, список отсортирован по
    /// CreatedOn (Доступно Админам)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetActive()
    {
        try
        {
            var users = await _context.Users
                .Where(u => u.RevokedOn.HasValue)
                .OrderBy(u => u.CreatedOn)
                .ToIndexModel()
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{login}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUser([FromRoute(Name = "login")] string login)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Login.Equals(login));
        if (user != null)
            return Ok(user.ToIndexModel());
        return BadRequest("No such user");
    }

    /// <summary>
    /// 7) Запрос пользователя по логину и паролю (Доступно только самому пользователю, если он
    /// активен (отсутствует RevokedOn))
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [Route("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Me()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 8) Запрос всех пользователей старше определённого возраста (Доступно Админам)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOlderThan([FromQuery] int? olderThan)
    {
        if (olderThan is null or <= 0 or > 100)
            return BadRequest("Invalid age");
        try
        {
            var users = await _context.Users
                .OlderThan(olderThan.Value)
                .ToIndexModel()
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    #endregion

}