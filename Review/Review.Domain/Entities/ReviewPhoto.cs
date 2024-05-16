using Starly.Domain.Interfaces.Entities;

namespace Review.Domain.Entities;

public class ReviewPhoto : IEntity<int>
{
    public int Id { get; set; }
    public int ReviewId { get; set; }
    public string Url { get; set; }
}
