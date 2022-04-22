using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Models;

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
        throw new NotImplementedException();
    }
    
    public async Task<Account> FindByLoginAsync(string login)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> IsAdminAsync(string login)
    {
        throw new NotImplementedException();
    }
}

public class IdentityResult
{
    public bool IsSuccess => throw new NotImplementedException();
    public IEnumerable<IdentityError> Errors => throw new NotImplementedException();
}

public class IdentityError
{
    public string Description { get; protected set; }
}