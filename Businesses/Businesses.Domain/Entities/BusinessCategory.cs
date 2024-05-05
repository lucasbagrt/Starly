using Starly.Domain.Interfaces.Entities;

namespace Businesses.Domain.Entities;

public class BusinessCategory : IEntity<int>
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int BusinessId { get; set; }
    public virtual Category Categories { get;}
    public virtual Business Business { get;}
}
