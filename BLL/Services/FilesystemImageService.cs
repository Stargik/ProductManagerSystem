using System;
using BLL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using BLL.Interfaces;

namespace BLL.Services
{
    public class FilesystemImageService : IImageService
	{
        private readonly StaticFilesSettings imgSettings;

        public FilesystemImageService(IOptions<StaticFilesSettings> imgSettings)
        {
            this.imgSettings = imgSettings.Value;
        }

        public async Task<IFormFile> Download(string imgName)
        {
            string directoryPath = imgSettings.Path;
            string fullPath = directoryPath + "/" + imgName;
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                return new FormFile(stream, 0, stream.Length, imgName, imgName);
            }
        }

        public async Task<string> GetStoragePath()
        {
            string directoryPath = imgSettings.Path;
            return directoryPath;
        }

        public async Task Upload(IFormFile imgFile)
        {
            string directoryPath = imgSettings.Path;
            string fullPath = directoryPath + "/" + imgFile.FileName;
            string[] fileEntries = Directory.GetFiles(directoryPath);

            if (fileEntries.Contains(fullPath))
            {
                throw new Exception("File is already exist.");
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imgFile.CopyToAsync(stream);
            }
        }
    }
}

