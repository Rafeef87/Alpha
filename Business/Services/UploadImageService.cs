

//using Microsoft.AspNetCore.Http;

//namespace Business.Services;

//public class UploadImageService
//{
//    private readonly IWebHostEnvironment _webHostEnvironment;

//    public ImageService(IWebHostEnvironment webHostEnvironment)
//    {
//        _webHostEnvironment = webHostEnvironment;
//    }

//    public async Task<string?> SaveImageAsync(IFormFile imageFile, string subfolder)
//    {
//        if (imageFile == null || imageFile.Length == 0)
//            return null;

//        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", subfolder);
//        Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

//        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
//        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//        using (var fileStream = new FileStream(filePath, FileMode.Create))
//        {
//            await imageFile.CopyToAsync(fileStream);
//        }

//        return Path.Combine("images", subfolder, uniqueFileName).Replace("\\", "/");
//    }
//}
//}
