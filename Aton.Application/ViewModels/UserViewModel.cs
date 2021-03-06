using Aton.Domain.Models;

namespace Aton.Application.ViewModels;

public class UserViewModel
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string RevokedBy { get; set; }
}