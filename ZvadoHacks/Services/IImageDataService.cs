using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Models;

namespace ZvadoHacks.Services
{
    public interface IImageDataService
    {
        Task<Stream> GetThumbnail(string id);

        Task<Stream> GetArticleFullscreen(string id);

        Task<Stream> GetArticlePreview(string id);

        Task<Stream> GetOriginal(string id);

        Task<ImageData> Update(ImageInputModel image);

        Task<List<string>> GetAllImages();
    }
}
