using Aton.Infrastructure.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Aton.Infrastructure.Identity.Managers;

public interface IUserService  
{  
    bool ValidateCredentials(string username, string password);  
}  
public class SignInManager  : IUserService
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