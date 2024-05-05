namespace Businesses.Domain.Dtos;

public class CreateBusinessDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Location { get; set; }
    public List<CreateBusinessCategoryDto> Categories { get; set; }
    public List<CreateBusinessHourDto> BusinessHour { get; set; }
}
