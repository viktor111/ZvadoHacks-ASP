using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Models;
using ZvadoHacks.Models.Home;

namespace ZvadoHacks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Comment> _commentRepository;


        public HomeController
            (
                ILogger<HomeController> logger, 
                IRepository<Article> articleRepository, 
                IRepository<Comment> commentRepository
            )
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.All();
            var comments = await _commentRepository.All();                       

            var model = new HomeIndexModel();
            model.ArticlesCount = articles.Count();
            model.CommentsCount = comments.Count();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
