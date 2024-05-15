namespace Businesses.Domain.Dtos;

public class CreateBusinessHourDto
{
    public string Start { get; set; }
    public string End { get; set; }
    public short DayOfWeek { get; set; }    
}
