using Aton.Infrastructure.Identity.Data;

namespace Aton.Infrastructure.Identity.Managers;

public class SignInManager
{
    private readonly AccountDbContext _accountContext;

    public SignInManager(AccountDbContext accountContext)
    {
        _accountContext = accountContext;
    }

    public bool ValidateCredentials(string login, string password)
    {
        var acc = _accountContext.Accounts.SingleOrDefault(a => a.Login.Equals(login));
        return acc != null && acc.Password.Equals(password);
    }

    
}