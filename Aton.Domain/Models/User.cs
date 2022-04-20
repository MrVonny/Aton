using Aton.Domain.Core.Models;

namespace Aton.Domain.Models;

public class User : EntityAudit
{
    public string Login { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }

    public override bool IsDeleted => RevokedAt.HasValue;
}