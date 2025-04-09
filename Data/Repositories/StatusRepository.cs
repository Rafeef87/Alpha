
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public class StatusRepository(DataContext context) : BaseRepository<StatusEntity, Status>(context)
{
}
