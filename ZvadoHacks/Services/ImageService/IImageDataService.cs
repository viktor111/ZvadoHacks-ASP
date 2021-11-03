using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZvadoHacks.Data;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Models;

namespace ZvadoHacks.Services.ImageService
{
    public interface IImageDataService
    {
        Task<Stream> GetThumbnail(string id);

        Task<Stream> GetArticleFullscreen(string id);

        Task<Stream> GetArticlePreview(string id);

        Task<Stream> GetOriginal(string id);

        Task<ImageData> GetUserImage(ApplicationUser user);

        Task<ImageData> Update(ImageInputModel image);

        Task<ImageData> UpdateForUser(ImageInputModel image);

        Task<List<string>> GetAllImages();
    }
}
