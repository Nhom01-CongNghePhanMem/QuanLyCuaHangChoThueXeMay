using Microsoft.AspNetCore.Http;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;

namespace MotorbikeRental.Infrastructure.ExternalServices.StorageService
{
    public class FileService : IFileService
    {
        private readonly string baseDirectory;
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private readonly long maxFileSize = 5 * 1024 * 1024;
        public FileService()
        {
            baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(baseDirectory);
        }
        public async Task<string> SaveImage(IFormFile formFile, string folder)
        {
            if (!IsValidImage(formFile))
                throw new Exception("File is invalid or empty.");
            string folderPath = Path.Combine(baseDirectory, folder);
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return Path.Combine("/uploads",folder, fileName).Replace("\\", "/");
        }
        public async Task<List<string>> SaveImages(IList<IFormFile> formFiles, string folder)
        {
            List<string> filePaths = new List<string>();
            for (int i = 0; i < formFiles.Count; i++)
            {
                if (IsValidImage(formFiles[i]))
                {
                    filePaths.Add(await SaveImage(formFiles[i], folder));
                }
            }
            return filePaths;
        }
        public bool IsValidImage(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return false;
            if (formFile.Length > maxFileSize)
                return false;
            string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
        public bool DeleteFile(string filePath)
        {
            string fullPath = Path.Combine(baseDirectory, filePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return false;
        }
        public bool FileExits(string filePath)
        {
            return File.Exists(Path.Combine(baseDirectory, filePath).Replace("/", "\\"));
        }
        public bool DeleteFiles(IList<string> filePaths)
        {
            bool allDeleted = true;
            for (int i = 0; i < filePaths.Count; i++)
            {
                string fullPath = Path.Combine(baseDirectory, filePaths[i]).Replace("/", "\\");
                if (!DeleteFile(fullPath))
                    allDeleted = false;
            }
            return allDeleted;
        }
        public string GetFileUrl(string relativePath)
        {
            return $"uploads/{relativePath}";
        }
    }
}