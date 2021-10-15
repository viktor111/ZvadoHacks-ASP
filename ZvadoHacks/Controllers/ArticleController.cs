using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Helpers;
using ZvadoHacks.Infrastructure.Filters;
using ZvadoHacks.Models;
using ZvadoHacks.Models.Article;
using ZvadoHacks.Services;

namespace ZvadoHacks.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IImageProcessorService _imageProcessor;
        private readonly IImageDataService _imageDataService;
        private readonly IRepository<Article> _articleRepository;

        public ArticleController
            (
                IImageProcessorService imageProcessor,
                IRepository<Article> articleRepository,
                IImageDataService imageDataService
            )
        {
            _imageProcessor = imageProcessor;
            _articleRepository = articleRepository;
            _imageDataService = imageDataService;
        }
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var guid = new Guid(id);
            var article = await _articleRepository.Get(guid);
            await _articleRepository.Delete(article);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var guid = new Guid(id);

            var article = await _articleRepository.Get(guid);

            var model = new ArticleDetailsModel();
            model.ArticleId = article.Id.ToString();
            model.ImageId = article.Image.Id.ToString();
            model.Heading = article.Heading;
            model.Author = article.Author;
            model.CreatedOn = article.CreatedOn;
            model.Content = article.Content;
            model.Comments = article.Comments.ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(ArticleInputModel articleInputModel, string id)
        {
            var guid = new Guid(id);
            var articleToSave = await _articleRepository.Get(guid);
            articleToSave.Heading = articleInputModel.Heading;
            articleToSave.Author = articleInputModel.Author;
            articleToSave.Content = articleInputModel.Content;
            articleToSave.CreatedOn = DateTime.Now;

            var article = await _articleRepository.Update(articleToSave);

            var imageInpuModel = new ImageInputModel();
            imageInpuModel.Name = articleInputModel.Image.FileName;
            imageInpuModel.Type = articleInputModel.Image.ContentType;
            imageInpuModel.Content = articleInputModel.Image.OpenReadStream();
            imageInpuModel.ArticleId = article.Id;

            await _imageDataService.Update(imageInpuModel);

            return RedirectToAction(nameof(Details), new { id = TempData["articleId"] });
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(string id)
        {
            TempData["articleId"] = id;
            var guid = new Guid(id);
            var article = await _articleRepository.Get(guid);

            ViewData["article"] = article;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All
            (
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber
            )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var articles = await _articleRepository.All();

            var model = articles.Select(a => new ArticlePreviewModel
            {
                PreviewImageId = a.Image.Id.ToString(),
                ArticleId = a.Id.ToString(),
                Heading = a.Heading,
                Author = a.Author,
                CreatedOn = a.CreatedOn,
                PreviewContent = ArticleContentTrimmer.Trim(a.Content),
                Content = a.Content                
            }).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Heading.Contains(searchString)
                                       || s.Content.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.Heading).ToList();
                    break;
                case "Date":
                    model = model.OrderBy(s => s.CreatedOn).ToList();
                    break;
                case "date_desc":
                    model = model.OrderByDescending(s => s.CreatedOn).ToList();
                    break;
                default:
                    model = model.OrderBy(s => s.Heading).ToList();
                    break;
            }            

            return View(model.ToPagedList(pageNumber ?? 1, 2));
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RequestSizeLimitChecker]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(ArticleInputModel articleInputModel)
        {
            var articleToSave = new Article();
            articleToSave.Heading = articleInputModel.Heading;
            articleToSave.Author = articleInputModel.Author;
            articleToSave.Content = articleInputModel.Content;
            articleToSave.CreatedOn = DateTime.Now;

            var article = await _articleRepository.Add(articleToSave);

            var imageInpuModel = new ImageInputModel();
            imageInpuModel.Name = articleInputModel.Image.FileName;
            imageInpuModel.Type = articleInputModel.Image.ContentType;
            imageInpuModel.Content = articleInputModel.Image.OpenReadStream();
            imageInpuModel.ArticleId = article.Id;

            await _imageProcessor.Process(imageInpuModel);

            return RedirectToAction(nameof(Create));
        }
    }
}
