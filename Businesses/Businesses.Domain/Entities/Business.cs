using Starly.Domain.Interfaces.Entities;

namespace Businesses.Domain.Entities;

public class Business : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; } = true;
    public virtual ICollection<BusinessPhoto> Photos { get; set; }
    public virtual ICollection<BusinessCategory> Categories { get; set; }
    public virtual ICollection<BusinessHour> Hours { get; set; }
}