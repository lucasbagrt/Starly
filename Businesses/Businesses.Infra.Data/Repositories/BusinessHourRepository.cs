using Businesses.Domain.Entities;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Infra.Data.Context;
using Starly.Infra.Data.Repositories;

namespace Businesses.Infra.Data.Repositories;

public class BusinessHourRepository(ApplicationDbContext context) : BaseRepository<BusinessHour, int, ApplicationDbContext>(context), IBusinessHourRepository
{
}
