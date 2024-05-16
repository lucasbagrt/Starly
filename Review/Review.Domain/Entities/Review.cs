using Starly.Domain.Interfaces.Entities;

namespace Review.Domain.Entities;

public class Review : IEntity<int>
{
    public int Id { get; set ; }
    public int UserId { get; set ; }    
    public string Comment { get; set; }
    public short Rating { get; set; }
}
