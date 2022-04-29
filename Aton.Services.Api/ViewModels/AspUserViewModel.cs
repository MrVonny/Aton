using Aton.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Aton.Services.Api.ViewModels;

//[SwaggerSchemaFilter(typeof(AspUserViewModelSchema))]
[SwaggerSchema(Title = "UserViewModel", ReadOnly = true)]
public class AspUserViewModel
{
    [SwaggerSchema("Name", Nullable = false)]
    public string Name { get; set; }
    [SwaggerSchema("Login", Nullable = false)]
    public string Login { get; set; }
    [SwaggerSchema("Gender", Nullable = false)]
    public Gender Gender { get; set; }
    [SwaggerSchema("Date of Birth", Format = "date", Nullable = true)]
    public DateTime? Birthday { get; set; }
    [SwaggerSchema("The date it was created", Nullable = false)]
    public DateTime CreatedAt { get; set; }
    [SwaggerSchema("Login of user who created it", Nullable = false)]
    public string CreatedBy { get; set; }
    [SwaggerSchema("The date it was edited", Nullable = false)]
    public DateTime UpdatedAt { get; set; }
    [SwaggerSchema("Login of user who edited it", Nullable = false)]
    public string UpdatedBy { get; set; }
    [SwaggerSchema("The date it was deleted", Nullable = true)]
    public DateTime? RevokedAt { get; set; }
    [SwaggerSchema("Login of user who deleted it", Nullable = true)]
    public string RevokedBy { get; set; }
}