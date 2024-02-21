using System;
using BLL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using BLL.Interfaces;
using System.IO;

namespace BLL.Services
{
    public class FilesystemImageService : IImageService
	{
        private readonly StaticFilesSettings imgSettings;
        private readonly string rootPath;

        public FilesystemImageService(IOptions<StaticFilesSettings> imgSettings, string rootPath)
        {
            this.imgSettings = imgSettings.Value;
            this.rootPath = rootPath;
        }

        public async Task<IFormFile> Download(string imgName)
        {
            string directoryPath = rootPath + "/" + imgSettings.Path;
            string fullPath = directoryPath + "/" + imgName;
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                return new FormFile(stream, 0, stream.Length, imgName, imgName);
            }
        }

        public async Task<string> GetStoragePath()
        {
            string directoryPath = "/" + imgSettings.Path;
            return directoryPath;
        }

        public async Task<string> GetAbsoluteStoragePath()
        {
            string directoryPath = rootPath + "/" + imgSettings.Path;
            return directoryPath;
        }

        public async Task Remove(string imgName)
        {
            string directoryPath = rootPath + "/" + imgSettings.Path;
            string fullPath = directoryPath + "/" + imgName;
            string[] fileEntries = Directory.GetFiles(directoryPath);
            if (fileEntries.Contains(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task Upload(IFormFile imgFile)
        {
            string directoryPath = rootPath + "/" + imgSettings.Path;
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

