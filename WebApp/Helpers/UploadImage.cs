using Microsoft.AspNetCore.Hosting;

namespace WebApp.Helpers;

// Chat GPT generated this code
public class UploadImage(IWebHostEnvironment webHostEnv)
{
    private readonly IWebHostEnvironment _webHostEnv = webHostEnv;

    public async Task<string?> UploadImageAsync(IFormFile imageFile, string folderName)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        string uploadsFolder = Path.Combine(_webHostEnv.WebRootPath, "images", folderName);
        Directory.CreateDirectory(uploadsFolder); // Create the folder if it does not exist 

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        return uniqueFileName; // You can save this in the database
    }
}
