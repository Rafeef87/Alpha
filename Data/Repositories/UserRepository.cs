
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity, User>(context)
{
}
