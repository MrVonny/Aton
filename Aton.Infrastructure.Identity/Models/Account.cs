using Aton.Domain.Core.Models;

namespace Aton.Infrastructure.Identity.Models;

public class Account : EntityAudit
{
    public Account(string login, string passwordHash, bool admin = false)
    {
        Login = login;
        PasswordHash = passwordHash;
        Admin = admin;
    }
    
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public bool Admin { get; protected set; }
    public AccountToUser AccountToUser { get; set; }
}

