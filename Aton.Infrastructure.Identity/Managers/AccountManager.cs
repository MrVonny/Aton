using System.Security.Claims;
using Aton.Domain.Services.Hash;
using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Models;
using Aton.Infrastructure.Identity.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Managers;

public class AccountManager
{
    private readonly AccountDbContext _accountContext;
    private readonly IPasswordHasher _passwordHasher;
    public string CurrentUser { get; set; }

    public AccountManager(AccountDbContext accountContext, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordHasher)
    {
        _accountContext = accountContext;
        _passwordHasher = passwordHasher;
        CurrentUser = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
    }

    protected bool ValidateLogin(string login, out ValidationResult validationResult)
    {
        validationResult = new LoginValidator().Validate(login);
        return validationResult.IsValid;
    }
    
    protected bool ValidatePassword(string password, out ValidationResult validationResult)
    {
        validationResult = new PasswordValidator().Validate(password);
        return validationResult.IsValid;
    }

    public async Task<IdentityResult> CreateAsync(CreateAccountViewModel createAccountViewModel)
    {
        if (!ValidateLogin(createAccountViewModel.Login, out var validationResult))
            return new IdentityResult(validationResult.Errors);
        if (!ValidatePassword(createAccountViewModel.Password, out validationResult))
            return new IdentityResult(validationResult.Errors);
        
        var find = await FindByLoginAsync(createAccountViewModel.Login);
        if (find != null)
            return new IdentityResult(new IdentityError("Already exists"));

        var passwordHash = _passwordHasher.Hash(createAccountViewModel.Password);
        _accountContext.Accounts.Add(new Account(createAccountViewModel.Login, passwordHash, createAccountViewModel.Admin));
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    public async Task<IdentityResult> MapToUser(string login, Guid userId)
    {
        if (!ValidateLogin(login, out var validationResult))
            return new IdentityResult(validationResult.Errors);
        
        var account = await FindByLoginAsync(login);
        if (account == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        account.AccountToUser = new AccountToUser(account.Id, userId);
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    protected async Task<Account> FindByLoginAsync(string login)
    {
        return await _accountContext.Accounts.SingleOrDefaultAsync(a=>a.Login.Equals(login));
    }
    
    public async Task<bool> IsAdminAsync(string login)
    {
        var acc = await FindByLoginAsync(login);
        return acc.Admin;
    }

    public async Task<Guid?> GetUserGuid(string login)
    {
        var atc = await _accountContext.Accounts
            .Include(a => a.AccountToUser)
            .SingleOrDefaultAsync(a => a.Login.Equals(login));
        return atc?.AccountToUser?.UserId;
    }
    
    public async Task<string> GetUserLogin(Guid guid)
    {
        var atc = await _accountContext.AccountToUser
            .Include(atu => atu.Account)
            .SingleOrDefaultAsync(atu => atu.UserId.Equals(guid));
        return atc?.Account.Login;
    }

    public async Task<IdentityResult> ChangeLogin(string login, string newLogin)
    {
        if (!ValidateLogin(login, out var validationResult))
            return new IdentityResult(validationResult.Errors);
        if (!ValidateLogin(newLogin, out validationResult))
            return new IdentityResult(validationResult.Errors);
        
        var account = _accountContext.Accounts
            .SingleOrDefault(a=>a.Login.Equals(login));
        
        if (account == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        if(await _accountContext.Accounts.AnyAsync(a=>a.Login.Equals(newLogin)))
            return new IdentityResult(new IdentityError("Login already taken"));
        
        account.Login = newLogin;
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    public async Task<IdentityResult> ChangePassword(string login, string newPassword)
    {
        if (!ValidateLogin(login, out var validationResult))
            return new IdentityResult(validationResult.Errors);
        if (!ValidatePassword(newPassword, out  validationResult))
            return new IdentityResult(validationResult.Errors);
        
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        
        acc.PasswordHash = _passwordHasher.Hash(newPassword);
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }
    public async Task<IdentityResult> RevokeAsync(string login)
    {
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        acc.RevokedAt = DateTime.Now;
        acc.RevokedBy = CurrentUser;
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    public async Task<IdentityResult> Remove(string login)
    {
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        _accountContext.Remove(acc);
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    public async Task<IdentityResult> RestoreAsync(string login)
    {
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        acc.RevokedAt = null;
        acc.RevokedBy = null;
        await _accountContext.SaveChangesWithUserAsync(CurrentUser);
        return new IdentityResult();
    }

    public async Task<bool> IsActiveAsync(string login)
    {
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return false;
        return !acc.RevokedAt.HasValue;
    }
}

public class IdentityResult
{
    public IdentityResult(params IdentityError[] errors)
    {
        Errors = errors;
    }

    public IdentityResult(IEnumerable<ValidationFailure> vaildationErrors)
    {
        Errors = vaildationErrors.Select(v => new IdentityError(v.ErrorMessage));
    }

    public bool IsSuccess => !Errors.Any();
    public IEnumerable<IdentityError> Errors { get; protected set; }
}

public class IdentityError
{
    public IdentityError(string description)
    {
        Description = description;
    }
    public string Description { get; protected set; }
}

public class CreateAccountViewModel
{
    public CreateAccountViewModel(string login, string password, bool admin = false)
    {
        Login = login;
        Password = password;
        Admin = admin;
    }
    
    public string Login { get; protected set; }
    public string Password { get; protected set; }
    public bool Admin { get; protected set; }
}