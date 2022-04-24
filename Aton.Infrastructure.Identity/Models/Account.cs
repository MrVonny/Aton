using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aton.Domain.Core.Models;

namespace Aton.Infrastructure.Identity.Models;

public class Account : EntityAudit
{
    public Account(string login, string password, bool admin = false)
    {
        Login = login;
        Password = password;
        Admin = admin;
    }
    
    public string Login { get; set; }
    public string Password { get; set; }
    public bool Admin { get; protected set; }
    public AccountToUser AccountToUser { get; set; }
}

