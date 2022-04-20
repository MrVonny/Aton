namespace Aton.Infrastructure.Identity.Models;

public class EditAccountLoginModel
{
    //[FromRoute(Name = "login")]
    public string Login { get; set; }
    public string NewLogin { get; set; }
}