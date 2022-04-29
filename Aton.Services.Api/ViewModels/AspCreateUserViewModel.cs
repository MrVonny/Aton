using Aton.Domain.Models;

namespace Aton.Services.Api.ViewModels;

public class AspCreateUserViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool? Admin { get; set; }
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; } 
    
}