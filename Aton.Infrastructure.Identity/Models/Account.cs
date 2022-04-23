using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Aton.Infrastructure.Identity.Models;

public class Account
{
    public Account(string login, string password, bool admin = false)
    {
        Login = login;
        Password = password;
        Admin = admin;
    }
    
    public Account(string login, string password, Guid userId, bool admin = false)
    {
        Login = login;
        Password = password;
        Admin = admin;
        AccountToUser = new AccountToUser(login, userId);
    }
    [Key]
    public string Login { get; set; }
    public string Password { get; set; }
    public bool Admin { get; protected set; }
    public AccountToUser AccountToUser { get; set; }
}

