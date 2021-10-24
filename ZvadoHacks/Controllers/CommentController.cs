using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZvadoHacks.Data;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Models.CommentModels;

namespace ZvadoHacks.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Comment> _commentRepository;

        public CommentController
            (
                IRepository<Comment> commentRepository,
                UserManager<ApplicationUser> userManager
            )
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var guid = new Guid(id);
            var comment = await _commentRepository.Get(guid);

            await _commentRepository.Delete(comment);

            return RedirectToAction("Details", "Article");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentInputModel inputModel, string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var articleId = new Guid(id);

            var comment = new Comment();
            comment.Content = inputModel.Content;
            comment.ArticleId = articleId;
            comment.UserId = user.Id;
            comment.CreatedOn = DateTime.Now;

            await _commentRepository.Add(comment);

            return RedirectToAction("Details", "Article", new { id = id });
        }
    }
}
