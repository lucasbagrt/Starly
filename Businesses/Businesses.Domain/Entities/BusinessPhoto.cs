using Starly.Domain.Interfaces.Entities;

namespace Businesses.Domain.Entities;

public class BusinessPhoto : IEntity<int>
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string PhotoUrl { get; set; }
    public bool Default { get; set;}
    public DateTime CreatedAt { get; set;}
}