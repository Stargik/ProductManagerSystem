using System;
using DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
	public interface IImageService
	{
        public Task Upload(IFormFile imgFile);
        public Task<IFormFile> Download(string imgName);
        public Task<string> GetStoragePath();
    }
}

