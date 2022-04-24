using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Guid { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool Admin { get; protected set; }
    public AccountToUser AccountToUser { get; set; }
}

