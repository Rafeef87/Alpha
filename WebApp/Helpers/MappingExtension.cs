using Domain.Models;
using WebApp.Models;

namespace WebApp.Helpers;

public static class MappingExtension
{
    public static AddProjectFormData MapToAddProjectFormData(this AddProjectViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

        return new AddProjectFormData
        {
            Image = viewModel.ImageFile != null ? ConvertImageToString(viewModel.ImageFile) : null, // Implement ConvertImageToString as needed
            ProjectName = viewModel.ProjectName,
            Description = viewModel.Description,
            StartDate = viewModel.StartDate,
            EndDate = viewModel.EndDate,
            Budget = viewModel.Budget,
            ClientId = viewModel.ClientName,
            UserId = string.Join(",", viewModel.SelectedMemberIds), // Assuming you want to store selected member IDs as a comma-separated string
            StatusId = 1 // Assuming a default status ID; adjust as necessary

        };
    }

    // New method to map from EditProjectViewModel to EditProjectFormData
    public static EditProjectFormData MapToEditProjectFormData(this EditProjectViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

        return new EditProjectFormData
        {
            // Convert the Id from string to int as EditProjectFormData.Id is an int
          
            Image = viewModel.ImageFile != null ? ConvertImageToString(viewModel.ImageFile) : null,
            ProjectName = viewModel.ProjectName,
            Description = viewModel.Description,
            EndDate = viewModel.EndDate,
            StartDate = viewModel.StartDate,
            Budget = viewModel.Budget,
            ClientId = viewModel.ClientName,
            UserId = string.Join(",", viewModel.SelectedMemberIds), // Assuming you want to store selected member IDs as a comma-separated string
            StatusId = 1 // Assuming a default status ID; adjust as necessary

        };
    }

    // Example helper method, if you need to convert IFormFile to a string path or base64 string.
    private static string ConvertImageToString(IFormFile imageFile)
    {
        // Implement conversion logic if needed.
        // For now, just return a placeholder or the file's name.
        return imageFile.FileName;
    }
}
