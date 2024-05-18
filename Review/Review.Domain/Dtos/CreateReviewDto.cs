namespace Review.Domain.Dtos;

public class CreateReviewDto
{
    public int BusinessId { get; set; }
    public string Comment { get; set; }
    public short Rating { get; set; }    
}
