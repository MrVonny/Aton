using Aton.Domain.Services.Hash;
using Aton.Infrastructure.Identity.Data;

namespace Aton.Infrastructure.Identity.Managers;

public class SignInManager
{
    private readonly AccountDbContext _accountContext;
    private readonly IPasswordHasher _passwordHasher;

    public SignInManager(AccountDbContext accountContext, IPasswordHasher passwordHasher)
    {
        _accountContext = accountContext;
        _passwordHasher = passwordHasher;
    }

    public bool ValidateCredentials(string login, string password)
    {
        var acc = _accountContext.Accounts.SingleOrDefault(a => a.Login.Equals(login));
        return acc != null && _passwordHasher.Check(acc.PasswordHash, password).Verified;
    }

    
}