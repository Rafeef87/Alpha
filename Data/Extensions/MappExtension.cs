
using System.Reflection;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Http;


namespace Data.Extensions;

public static class MappExtension
{
    public static TDestination MapTo<TDestination>(this object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        TDestination destination = Activator.CreateInstance<TDestination>()!;

        var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destinationProperties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var destinationProperty in destinationProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == destinationProperty.Name && x.PropertyType == destinationProperty.PropertyType);
            if (sourceProperty != null && destinationProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }
        }
        return destination;
    }









    // Extension method for mapping from AddProjectFormData to ProjectEntity
    public static ProjectEntity MapToProjectEntity(this AddProjectFormData formData)
    {
        ArgumentNullException.ThrowIfNull(formData, nameof(formData));

        return new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            ClientId = formData.ClientId,
            UserId = formData.UserId,
            Image = formData.Image
        };
    }
    public static ProjectEntity MapToProjectEntity(this Project project)
    {
        ArgumentNullException.ThrowIfNull(project, nameof(project));

        return new ProjectEntity
        {
            Id = project.Id,
            Image = project.Image,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            ClientId = project.Client?.Id ?? string.Empty,
            UserId = project.User?.Id ?? string.Empty,
            StatusId = project.Status.Id
        };
    }
    public static ProjectEntity MapToProjectEntity(this EditProjectFormData formData)
    {
        ArgumentNullException.ThrowIfNull(formData, nameof(formData));

        return new ProjectEntity
        {
            // Assuming Id is not updated from the form
            Id = formData.Id.ToString(),
            Image = formData.Image,
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            ClientId = formData.ClientId,
            UserId = formData.UserId,
            StatusId = formData.StatusId
        };
    }


    public static Project MapToProject(this ProjectEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Project
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Client = entity.Client.MapToClient(),
            Status = entity.Status?.MapToStatus()!,
            Users = new List<User> { entity.User.MapToUser() }
        };
    }


    public static Client MapToClient(this ClientEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Client
        {
            Id = entity.Id,
            ClientName = entity.ClientName
        };
    }

    public static Status MapToStatus(this StatusEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Status
        {
            Id = entity.Id,
            StatusName = entity.StatusName
        };
    }

    public static User MapToUser(this UserEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new User
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email!
        };

    }
}
