namespace Businesses.Domain.Dtos;

public class CreateBusinessHourDto
{
    public string Start { get; set; }
    public string End { get; set; }
    public short Day { get; set; }
    public bool IsOvernight { get; set; }
}
