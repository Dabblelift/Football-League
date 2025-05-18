using System.ComponentModel.DataAnnotations;
using Football_League.Data.Models.Interfaces;

namespace Football_League.Shared.Entities
{
    public abstract class BaseEntity<TKey> : IEntityAuditInfo
    {
        [Key]
        public TKey Id { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
