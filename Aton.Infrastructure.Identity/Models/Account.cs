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
    [Key]
    public string Login { get; protected set; }
    public string Password { get; protected set; }
    public bool Admin { get; protected set; }
}

