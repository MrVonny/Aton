using Microsoft.AspNetCore.Identity;

namespace Aton.Infrastructure.Identity.Models;

public class Account : IdentityUser
{
    public Account(string login, string password, bool admin = false)
    {
        Login = login;
        Password = password;
        Admin = admin;
    }
    
    public string Login { get; protected set; }
    public string Password { get; protected set; }
    public bool Admin { get; protected set; }
}

