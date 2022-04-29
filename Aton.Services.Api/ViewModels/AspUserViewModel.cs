using Aton.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Aton.Services.Api.ViewModels;

//[SwaggerSchemaFilter(typeof(AspUserViewModelSchema))]
[SwaggerSchema("UserViewModel")]
public class AspUserViewModel
{
    [SwaggerSchema("Name")]
    public string Name { get; set; }
    [SwaggerSchema("Login")]
    public string Login { get; set; }
    [SwaggerSchema("Gender")]
    public Gender Gender { get; set; }
    [SwaggerSchema("Date of Birth", Format = "date")]
    public DateTime? Birthday { get; set; }
    [SwaggerSchema("The date it was created")]
    public DateTime CreatedAt { get; set; }
    [SwaggerSchema("Login of user who created it")]
    public string CreatedBy { get; set; }
    [SwaggerSchema("The date it was edited")]
    public DateTime UpdatedAt { get; set; }
    [SwaggerSchema("Login of user who edited it")]
    public string UpdatedBy { get; set; }
    [SwaggerSchema("The date it was deleted")]
    public DateTime? RevokedAt { get; set; }
    [SwaggerSchema("Login of user who deleted it")]
    public string RevokedBy { get; set; }
}