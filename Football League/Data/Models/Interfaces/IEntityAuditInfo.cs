namespace Football_League.Data.Models.Interfaces
{
    public interface IEntityAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
