using Starly.Domain.Interfaces.Entities;

namespace Businesses.Domain.Entities;

public class BusinessHour : IEntity<int>
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string Start {  get; set; }
    public string End { get; set; }
    public short DayOfWeek { get; set; }
    public bool IsOvernight { get; set; }
}
