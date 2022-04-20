namespace Aton.Infrastructure.Identity.Models;

public class EditAccountPasswordModel
{
    //[FromRoute(Name = "login")]
    public string Login { get; set; }
    public string NewPassword { get; set; }
}