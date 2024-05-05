using Starly.Domain.Interfaces.Entities;

namespace Businesses.Domain.Entities;

public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<BusinessCategory> BusinessCategories { get;}
}
