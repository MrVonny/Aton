using System.ComponentModel.DataAnnotations;
using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class CreateUserViewModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    [MinLength(3, ErrorMessage = "")]
    [MaxLength(25, ErrorMessage = "")]
    public string Login { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    [MinLength(8, ErrorMessage = "")]
    [MaxLength(25, ErrorMessage = "")]
    public string Password { get; set; }

    [Required]
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string Name { get; set; }
    
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; } 
    public bool? Admin { get; set; }
}