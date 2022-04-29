using Aton.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Aton.Services.Api.ViewModels;

[SwaggerSchema("CreateUserViewModel")]
public class AspCreateUserViewModel
{
    [SwaggerSchema("Login", Nullable = false)]
    public string Login { get; set; }
    [SwaggerSchema("Password", Nullable = false)]
    public string Password { get; set; }
    [SwaggerSchema("Is Admin", Nullable = true)]
    public bool? Admin { get; set; }
    [SwaggerSchema("Name", Nullable = false)]
    public string Name { get; set; }
    [SwaggerSchema("Gender", Nullable = false)]
    public Gender? Gender { get; set; }
    [SwaggerSchema("Date of Birth", Format = "date", Nullable = true)]
    public DateTime? Birthday { get; set; } 
    
}