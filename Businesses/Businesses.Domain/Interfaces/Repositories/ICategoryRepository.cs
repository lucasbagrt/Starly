using Businesses.Domain.Entities;
using Starly.Domain.Interfaces.Repositories;

namespace Businesses.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, int>
{
}
