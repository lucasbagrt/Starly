using Starly.Domain.Filters;

namespace Review.Domain.Filters;

public class ReviewFilter : _BaseFilter
{
    public int? BusinessId { get; set; }
}
