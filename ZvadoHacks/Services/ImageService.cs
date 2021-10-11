using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Models;

namespace ZvadoHacks.Services
{
    public class ImageService : IImageDataService, IImageProcessorService
    {

        private const int ThumbnailWidth = 300;
        private const int ArticlePreviewWidth = 600;
        private const int ArticleFullscreenWidth = 1000;

        private readonly IServiceScopeFactory _serviceFactory;
        private readonly ApplicationDbContext _dbContext;

        public ImageService(IServiceScopeFactory serviceFactory, ApplicationDbContext dbContext)
        {
            _serviceFactory = serviceFactory;
            _dbContext = dbContext;
        }

        public Task<List<string>> GetAllImages()
        {
            var result = _dbContext.Images.Select(i => i.Id.ToString()).ToListAsync();

            return result;
        }

        public async Task<ImageData> Update(ImageInputModel image)
        {
            using var imageResult = await Image.LoadAsync(image.Content);

            var original = await SaveImage(imageResult, imageResult.Width);
            var thumbnail = await SaveImage(imageResult, ThumbnailWidth);
            var articelPreview = await SaveImage(imageResult, ArticlePreviewWidth);
            var articleFullscreen = await SaveImage(imageResult, ArticleFullscreenWidth);

            var dbContext = _serviceFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

            var entityToRemove = await dbContext.Images.FirstOrDefaultAsync(i => i.ArticleId == image.ArticleId);
            dbContext.Remove(entityToRemove);

            var imageModel = new ImageData();
            imageModel.OriginalFileName = image.Name;
            imageModel.OriginalType = image.Type;
            imageModel.OriginalContent = original;
            imageModel.ThumbnailContent = thumbnail;
            imageModel.ArticlePreviewContent = articelPreview;
            imageModel.ArticleFullscreenContent = articleFullscreen;
            imageModel.ArticleId = image.ArticleId;

            var updatedEntity = dbContext.Images.Add(imageModel);

            await dbContext.SaveChangesAsync();

            return updatedEntity.Entity;
        }

        public Task<Stream> GetArticleFullscreen(string id)
        {
            return GetImageData(id, "ArticleFullscreen");
        }

        public Task<Stream> GetArticlePreview(string id)
        {
            return GetImageData(id, "ArticlePreview");
        }
        public Task<Stream> GetThumbnail(string id)
        {
            return GetImageData(id, "Thumbnail");
        }
        public Task<Stream> GetOriginal(string id)
        {
            return GetImageData(id, "Original");
        }
        

        public async Task Process(ImageInputModel image)
        {
            try
            {
                await Processor(image);
            }
            catch
            {

            }           
        }

        public async Task Process(IEnumerable<ImageInputModel> images)
        {
            var tasks = images
            .Select(image => Task.Run(async () =>
            {
                try
                {
                    await Processor(image);
                }
                catch
                {

                }
            }));

            await Task.WhenAll(tasks);
        }

        public async Task ProcessUser(ImageInputModel image)
        {
            using var imageResult = await Image.LoadAsync(image.Content);

            var original = await SaveImage(imageResult, imageResult.Width);
            var thumbnail = await SaveImage(imageResult, ThumbnailWidth);

            var dbContext = _serviceFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

            var imageModel = new ImageData();
            imageModel.OriginalFileName = image.Name;
            imageModel.OriginalType = image.Type;
            imageModel.OriginalContent = original;
            imageModel.ThumbnailContent = thumbnail;
            imageModel.UserId = image.UserId;

            await dbContext.Images.AddAsync(imageModel);
            await dbContext.SaveChangesAsync();
        }

        private async Task Processor(ImageInputModel image)
        {
            using var imageResult = await Image.LoadAsync(image.Content);

            var original = await SaveImage(imageResult, imageResult.Width);
            var thumbnail = await SaveImage(imageResult, ThumbnailWidth);
            var articelPreview = await SaveImage(imageResult, ArticlePreviewWidth);
            var articleFullscreen = await SaveImage(imageResult, ArticleFullscreenWidth);

            var dbContext = _serviceFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

            var imageModel = new ImageData();
            imageModel.OriginalFileName = image.Name;
            imageModel.OriginalType = image.Type;
            imageModel.OriginalContent = original;
            imageModel.ThumbnailContent = thumbnail;
            imageModel.ArticlePreviewContent = articelPreview;
            imageModel.ArticleFullscreenContent = articleFullscreen;
            imageModel.ArticleId = image.ArticleId;


            await dbContext.Images.AddAsync(imageModel);
            await dbContext.SaveChangesAsync();
        }

        private async Task<Stream> GetImageData(string id, string size)
        {
            var database = _dbContext.Database;

            var dbConnection = (SqlConnection)database.GetDbConnection();

            var command = new SqlCommand(
                $"SELECT {size}Content FROM Images WHERE Id = @id",
                dbConnection
                );

            command.Parameters.Add(new SqlParameter("@id", id));

            await dbConnection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            Stream result = null;

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    result = reader.GetStream(0);
                }
            }

            await reader.CloseAsync();
            await dbConnection.CloseAsync();

            return result;
        }

        private async Task<byte[]> SaveImage(Image image, int resizeWidth)
        {
            var width = image.Width;
            var height = image.Height;

            if (width > resizeWidth)
            {
                height = (int)((double)resizeWidth / width * height);
                width = resizeWidth;
            }

            image
                .Mutate(i => i
                    .Resize(new Size(width, height)));

            image.Metadata.ExifProfile = null;

            var memoryStream = new MemoryStream();


            await image.SaveAsJpegAsync(memoryStream, new JpegEncoder
            {
                Quality = 75
            });

            return memoryStream.ToArray();
        }        
    }
}
