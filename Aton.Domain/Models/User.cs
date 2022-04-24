using Aton.Domain.Core.Models;

namespace Aton.Domain.Models;

public class User : EntityAudit
{
    public User(Guid id, string name, Gender gender, DateTime? birthday = null)
    {
        Id = id;
        Name = name;
        Gender = gender;
        Birthday = birthday;
    }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }

    public override bool IsDeleted => RevokedAt.HasValue;
}