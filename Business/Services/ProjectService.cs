
using Business.Models;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository)
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (orderByDescending: true, storBy: s => s.Created, where: null,
            include => include.User,
            include => include.Status,
            include => include.Client
            );
        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            ( where: x => x.Id == id,
            include => include.User,
            include => include.Status,
            include => include.Client
            );
        return response.Succeeded
           ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
           : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }
}
