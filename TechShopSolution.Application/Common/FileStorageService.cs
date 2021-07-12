using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TechShopSolution.Application.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private readonly string _assetsFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        private const string Assets = "Assets";
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
            _assetsFolder = Path.Combine(webHostEnvironment.WebRootPath, Assets);
        }
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if(File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            return false;
        }

        public string GetFileUrl(string fileName)
        {
            string filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
                return filePath;
            return Path.Combine(_assetsFolder, "notfound.png");
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public bool isExistFile(string filename)
        {
            var filePath = Path.Combine(_userContentFolder, filename);
            if (File.Exists(filePath))
            {
                return true;
            }
            return false;
        }
    }
}
