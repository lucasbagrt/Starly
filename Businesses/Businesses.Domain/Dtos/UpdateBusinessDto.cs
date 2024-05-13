namespace Businesses.Domain.Dtos;

public class UpdateBusinessDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Location { get; set; }
    public List<CreateBusinessCategoryDto> Categories { get; set; }
    public List<CreateBusinessHourDto> Hours { get; set; }
}
