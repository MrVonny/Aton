using System.ComponentModel.DataAnnotations;
using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class UserViewModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    [MinLength(3, ErrorMessage = "")]
    [MaxLength(25, ErrorMessage = "")]
    public string Login { get; set; }

    [Required]
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string Name { get; set; }
    
    public Gender Gender { get; set; }
    //[Range(typeof(DateTime), "1/1/1900", null)]
    public DateTime? Birthday { get; set; } 
    public bool? Admin { get; set; }
}