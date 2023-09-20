using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebApi.Services
{
    public interface IFileService
    {
        Task SaveImage(string base64Image, string name);
    }

    public class FileService : IFileService
    {
        const string PATH_TO_IMAGES = "wwwroot\\images";
        public async Task SaveImage(string base64Image, string name)
        {
            var fileName = Path.Combine(PATH_TO_IMAGES, name);

            byte[] bytes = Convert.FromBase64String(base64Image);

            using (FileStream fileStream = File.Open(fileName, FileMode.Create))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }

}