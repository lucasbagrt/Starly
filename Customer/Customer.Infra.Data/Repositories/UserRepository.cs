using Customer.Domain.Entities;
using Customer.Domain.Interfaces.Repositories;
using Customer.Infra.Data.Context;
using Starly.Infra.Data.Repositories;

namespace Customer.Infra.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User, int, ApplicationDbContext>(context), IUserRepository
{
}