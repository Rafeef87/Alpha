using System.ComponentModel.DataAnnotations;
using Business.Models;
using Domain.Models;

namespace WebApp.Models;

public class ProjectsViewModel
{
    public ProjectResult<IEnumerable<Project>>? Projects;
}