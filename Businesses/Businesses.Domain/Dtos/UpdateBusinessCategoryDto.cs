namespace Businesses.Domain.Dtos;

public class UpdateBusinessCategoryDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int CategoryId { get; set; }
}
