namespace Aton.Domain.Core.Models
{
    public abstract class EntityAudit : Entity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string RevokedBy { get; set; }
    }
}
