using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aton.Models.Identity;

public class User : IdentityUser
{
    public string Login { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; } 
    public bool Admin { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string RevokedBy { get; set; }
}

public class CreateUserModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    [MinLength(3, ErrorMessage = "")]
    [MaxLength(25, ErrorMessage = "")]
    public string Login { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    public string Password { get; set; }
    [MinLength(8, ErrorMessage = "")]
    [MaxLength(25, ErrorMessage = "")]
    
    [Required]
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string UserName { get; set; }
    
    public Gender? Gender { get; set; }
    //[Range(typeof(DateTime), "1/1/1900", null)]
    public DateTime? Birthday { get; set; } 
    public bool? Admin { get; set; }

}

public class IndexUserModel
{
    public string Id { get; set; }
    public string Login { get; set; }
    public string UserName { get; set; }
    public bool Admin { get; set; }
    
    public DateTime CreatedOn { get; set; }
}

public class EditUserInfoModel
{
    [FromRoute(Name = "login")]
    public string Id { get; set; }
    [RegularExpression(@"[ЁёА-яa-zA-Z]")]
    public string UserName { get; set; }
    public Gender Gender { get; set; }
    //[Range(typeof(DateTime), "1/1/1900", null)]
    public DateTime? Birthday { get; set; } 
}

public class EditUserPasswordModel
{
    [FromRoute(Name = "login")]
    public string Login { get; set; }
    public string NewPassword { get; set; }
}

public class EditUserLoginModel
{
    [FromRoute(Name = "login")]
    public string Login { get; set; }
    public string NewLogin { get; set; }
}

public enum Gender
{
    Female,
    Male,
    Unknown
}