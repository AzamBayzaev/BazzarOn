using Ardalis.Specification.EntityFrameworkCore;
using BazzarOn.Domain.Entities;
using BazzarOn.Application.User.Repositories;

namespace BazzarOn.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository 
{
   public UserRepository(BazzarDbContext db) : base(db) {}
}