using Aton.Domain.Models;
using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Managers;

public class AccountManager
{
    private readonly AccountDbContext _accountContext;

    public AccountManager(AccountDbContext accountContext)
    {
        _accountContext = accountContext;
    }

    public async Task<IdentityResult> CreateAsync(Account account)
    {
        var find = await _accountContext.Accounts.FindAsync(account.Login);
        if (find != null)
            return new IdentityResult(new IdentityError("Already exists"));
        _accountContext.Accounts.Add(account);
        await _accountContext.SaveChangesAsync();
        return new IdentityResult();
    }

    public async Task<Account> FindByLoginAsync(string login)
    {
        return await _accountContext.Accounts.FindAsync(login);
    }
    
    public async Task<bool> IsAdminAsync(string login)
    {
        var acc = await FindByLoginAsync(login);
        return acc.Admin;
    }

    public async Task<Guid?> GetGuid(string login)
    {
        var atc = await _accountContext.AccountToUser.SingleOrDefaultAsync(atu => atu.AccountLogin.Equals(login));
        return atc?.UserId;
    }

    public async Task<IdentityResult> ChangeLogin(string login, string newLogin)
    {
        var account = await FindByLoginAsync(login);
        if (account == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        if(await _accountContext.Accounts.AnyAsync(a=>a.Login.Equals(newLogin)))
            return new IdentityResult(new IdentityError("Login already taken"));
        account.Login = newLogin;
        await _accountContext.SaveChangesAsync();
        return new IdentityResult();
    }

    public async Task<IdentityResult> ChangePassword(string login, string newPassword)
    {
        var acc = await FindByLoginAsync(login);
        if (acc == null)
            return new IdentityResult(new IdentityError("Account doesn't exists"));
        acc.Password = newPassword;
        await _accountContext.SaveChangesAsync();
        return new IdentityResult();
    }
}

public class IdentityResult
{
    public IdentityResult(params IdentityError[] errors)
    {
        Errors = errors;
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