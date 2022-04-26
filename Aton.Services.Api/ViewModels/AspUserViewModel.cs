using Aton.Domain.Models;

namespace Aton.Services.Api.ViewModels;

public class AspUserViewModel
{
    public string Name { get; set; }
    public string Login { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string RevokedBy { get; set; }
}