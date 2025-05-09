﻿
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
            // Map simple properties
            var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == destinationProperty.Name && x.PropertyType == destinationProperty.PropertyType);
            if (sourceProperty != null && destinationProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }

            // Map nested class properties
            var sourceClassProperty = sourceProperties.FirstOrDefault(x =>
                x.Name == destinationProperty.Name &&
                x.PropertyType.IsClass &&
                destinationProperty.PropertyType.IsClass &&
                x.PropertyType != typeof(string) &&
                destinationProperty.PropertyType != typeof(string));

            if (sourceClassProperty != null && destinationProperty.CanWrite)
            {
                var sourceValue = sourceClassProperty.GetValue(source);
                if (sourceValue == null)
                    continue;

                var mapMethod = typeof(MappExtension).GetMethod(nameof(MapTo))?.MakeGenericMethod(destinationProperty.PropertyType);
                if (mapMethod == null)
                    continue;

                var mappedValue = mapMethod.Invoke(null, new[] { sourceValue });
                destinationProperty.SetValue(destination, mappedValue);
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
            ClientId = formData.ClientName, 
            UserId = string.Join(",", formData.SelectedMemberIds), 
            Image = formData.Image
        };
    }
    public static ProjectEntity MapToProjectEntity(this EditProjectFormData formData)
    {
        ArgumentNullException.ThrowIfNull(formData, nameof(formData));

        return new ProjectEntity
        {
            Id = formData.Id.ToString(), // Fix: Convert int to string
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            ClientId = formData.ClientName,
            UserId = string.Join(",", formData.SelectedMemberIds),
            Image = formData.Image
        };
    }
    //public static ProjectEntity MapToProjectEntity(this Project project)
    //{
    //    ArgumentNullException.ThrowIfNull(project, nameof(project));

    //    return new ProjectEntity
    //    {
    //        Image = project.Image,
    //        ProjectName = project.ProjectName,
    //        Description = project.Description,

    //        Budget = project.Budget,
    //        ClientId = project.ClientName,
    //        UserId = string.Join(",", project.Users.Select(user => user.Id)) // Fix: Convert List<User> to a comma-separated string of User IDs
    //    };
    //}


    //public static Project MapToProject(this ProjectEntity entity)
    //{
    //    ArgumentNullException.ThrowIfNull(entity);

    //    return new Project
    //    {
    //        ProjectName = entity.ProjectName,
    //        Description = entity.Description,
    //        StartDate = entity.StartDate,
    //        EndDate = entity.EndDate,
    //        Budget = entity.Budget,
    //        ClientName = entity.Client.ClientName, // Fix: Map ClientName from ClientEntity  
    //        Users = new List<User> { entity.User.MapToUser() }
    //    };
    //}


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
            
        };

    }
}
