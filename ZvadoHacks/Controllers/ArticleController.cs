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
using ZvadoHacks.Models.ArticleModels;
using ZvadoHacks.Models.CommentModels;
using ZvadoHacks.Services.ImageService;

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

            var model = new ArticleDetailsModel
            {
                ArticleId = article.Id.ToString(),
                ImageId = article.Image.Id.ToString(),
                Heading = article.Heading,
                Author = article.Author,
                CreatedOn = article.CreatedOn,
                Content = article.Content
            };

            var comments = new List<CommentModel>();
            
            foreach (var comment in article.Comments)
            {
                var commentModel = new CommentModel
                {
                    Id = comment.Id,
                    CreatedOn = comment.CreatedOn,
                    Content = comment.Content,
                    User = comment.User,
                    UserId = comment.UserId,
                    Article = comment.Article,
                    ArticleId = comment.ArticleId
                };

                var imageForComment = await _imageDataService.GetUserImage(comment.User);
                
                commentModel.ImageId = imageForComment.Id.ToString();
                
                comments.Add(commentModel);
            }

            model.Comments = comments;

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

            if (articleInputModel.Image == null)
                return RedirectToAction(nameof(Details), new {id = TempData["articleId"]});
            
            var imageInputModel = new ImageInputModel
           {
               Name = articleInputModel.Image.FileName,
               Type = articleInputModel.Image.ContentType,
               Content = articleInputModel.Image.OpenReadStream(),
               ArticleId = article.Id
           };

           await _imageDataService.Update(imageInputModel);

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
                model = model.Where(s => s.Heading.ToLower().Contains(searchString.ToLower())
                                       || s.Content.ToLower().Contains(searchString.ToLower())).ToList();
            }

            model = sortOrder switch
            {
                "name_desc" => model.OrderByDescending(s => s.Heading).ToList(),
                "Date" => model.OrderBy(s => s.CreatedOn).ToList(),
                "date_desc" => model.OrderByDescending(s => s.CreatedOn).ToList(),
                _ => model.OrderBy(s => s.Heading).ToList()
            };

            var articleAllModel = new ArticleAllModel
            {
                Articles = model.ToPagedList(pageNumber ?? 1, 2),
                IsAdmin = User.IsInRole("Admin")
            };

            return View(articleAllModel);
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
            var articleToSave = new Article
            {
                Heading = articleInputModel.Heading,
                Author = articleInputModel.Author,
                Content = articleInputModel.Content,
                CreatedOn = DateTime.Now
            };

            var article = await _articleRepository.Add(articleToSave);

            var imageInputModel = new ImageInputModel
            {
                Name = articleInputModel.Image.FileName,
                Type = articleInputModel.Image.ContentType,
                Content = articleInputModel.Image.OpenReadStream(),
                ArticleId = article.Id
            };

            await _imageProcessor.Process(imageInputModel);

            return RedirectToAction(nameof(Create));
        }
    }
}
