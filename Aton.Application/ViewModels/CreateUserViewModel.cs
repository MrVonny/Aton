using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class CreateUserViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; } 
    public bool? Admin { get; set; }
}