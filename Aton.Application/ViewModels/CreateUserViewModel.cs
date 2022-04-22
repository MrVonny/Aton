using System.ComponentModel.DataAnnotations;
using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class CreateUserViewModel
{
    // [Required(ErrorMessage = "")]
    // [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "")]
    // [MinLength(3, ErrorMessage = "")]
    // [MaxLength(25, ErrorMessage = "")]
    public string Login { get; set; }
    
    //[Required(ErrorMessage = "")]
    //[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "")]
    //[MinLength(8, ErrorMessage = "")]
    //[MaxLength(25, ErrorMessage = "")]
    public string Password { get; set; }

    //[Required(ErrorMessage = "")]
    //[RegularExpression(@"[ЁёА-яa-zA-Z]", ErrorMessage = "")]
    public string Name { get; set; }
    
    [Required]
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; } 
    public bool? Admin { get; set; }
}