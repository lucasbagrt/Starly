using Businesses.Domain.Entities;

namespace Businesses.Domain.Dtos;

public class UpdateBusinessDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public BusinessLocationDto Location { get; set; }
    public List<UpdateBusinessCategoryDto> Categories { get; set; }
    public List<BusinessHour> Hours { get; set; }
}
