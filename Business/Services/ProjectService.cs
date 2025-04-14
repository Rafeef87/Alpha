
using System.Linq.Expressions;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.SqlServer.Server;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult> DeleteProjectAsync(string id);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult> UpdateProjectAsync(EditProjectFormData formData);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    #region CRUD
    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };
        var projectEntity = formData.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync(
            orderByDescending: true,
            storBy: s => s.Created,
            where: null,
            nameof(ProjectEntity.User),  // Use string-based navigation property names
            nameof(ProjectEntity.Status),
            nameof(ProjectEntity.Client)
        );

        return new ProjectResult<IEnumerable<Project>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = response.Result
        };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync(
            where: x => x.Id == id,
            nameof(ProjectEntity.User),  // Use string-based navigation property names
            nameof(ProjectEntity.Status),
            nameof(ProjectEntity.Client)
        );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }

    public async Task<ProjectResult> UpdateProjectAsync(EditProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required field are supplied." };
        var projectEntity = formData.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.UpdateAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid project ID." };

        var projectResult = await _projectRepository.GetAsync(x => x.Id == id);
        if (!projectResult.Succeeded || projectResult.Result == null)
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
       
        var projectEntity = projectResult.Result.MapTo<ProjectEntity>();

        var result = await _projectRepository.DeleteAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
    #endregion
}
