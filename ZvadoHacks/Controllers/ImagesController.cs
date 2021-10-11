using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Services;

namespace ZvadoHacks.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IImageDataService _imageDataService;

        public ImagesController(IImageDataService imageDataService)
        {
            _imageDataService = imageDataService;
        }

        [HttpGet]
        public async Task<IActionResult> ArticlePreview(string id)
        {
            return ReturnImageCache(await _imageDataService.GetArticlePreview(id));
        }

        [HttpGet]
        public async Task<IActionResult> Original(string id)
        {
            return ReturnImageCache(await _imageDataService.GetOriginal(id));
        }

        [HttpGet]
        public async Task<IActionResult> Thumbnail(string id)
        {
            return ReturnImageCache(await _imageDataService.GetThumbnail(id));
        }

        [HttpGet]
        public async Task<IActionResult> ArticleFullscreen(string id)
        {
            return ReturnImageCache(await _imageDataService.GetArticleFullscreen(id));
        }

        private IActionResult ReturnImageCache(Stream image)
        {
            var headers = Response.GetTypedHeaders();

            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            headers.Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30));

            return File(image, "image/jpeg");
        }
    }
}
