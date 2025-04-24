
using Data.Context;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
}
public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    //public IEnumerable<Project> GetAllProjects()
    //{
    //    return _context.Projects.Select(p => new Project
    //    {
    //        ProjectName = p.ProjectName,
    //        ClientName = p.ClientId,
    //        Description = p.Description,
    //        StartDate = p.StartDate,
    //        EndDate = p.EndDate,
    //        Budget = p.Budget,
    //        Users = new List<User> { new User { Id = p.UserId } } // Fixed the issue by creating a single User object with the UserId
    //    }).ToList();
    //}
}
