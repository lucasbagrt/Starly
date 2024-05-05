using Businesses.Domain.Entities;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Infra.Data.Context;
using Starly.Infra.Data.Repositories;

namespace Businesses.Infra.Data.Repositories;

public class BusinessRepository(ApplicationDbContext context) : BaseRepository<Business, int, ApplicationDbContext>(context), IBusinessRepository
{
}
