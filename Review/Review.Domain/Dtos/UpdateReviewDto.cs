namespace Review.Domain.Dtos;

public class UpdateReviewDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string Comment { get; set; }
    public short Rating { get; set; }    
}
