using Aton.Domain.Models;

namespace Aton.Infrastructure.Identity.Models;

public class AccountToUser
{
    public  AccountToUser(Account account, User user)
    {
        Account = account;
        User = user;
        AccountLogin = account.Login;
        UserId = user.Id;
    }
    
    public  AccountToUser(string accountLogin, Guid userId)
    {
        AccountLogin = accountLogin;
        UserId = userId;
    }

    protected AccountToUser()
    {
        
    }
    
    public string AccountLogin { get; protected set; }
    public Account Account { get; protected set; }
    public Guid UserId { get; protected set; }
    public User User { get; protected set; }
}