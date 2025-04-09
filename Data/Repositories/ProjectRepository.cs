
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context)
{
}
