using Aton.Infrastructure.Identity.Data;

namespace Aton.Infrastructure.Identity.Managers;

public class SignInManager
{
    private readonly AccountDbContext _accountContext;

    public SignInManager(AccountDbContext accountContext)
    {
        _accountContext = accountContext;
    }
}