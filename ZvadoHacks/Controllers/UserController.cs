using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZvadoHacks.Constants;
using ZvadoHacks.Data;
using ZvadoHacks.Models;
using ZvadoHacks.Models.UserModels;
using ZvadoHacks.Services.ImageService;

namespace ZvadoHacks.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageDataService _imageDataService;

        public UserController
            (
                UserManager<ApplicationUser> userManager,
                IImageDataService imageDataService
            )
        {
            _userManager = userManager;
            _imageDataService = imageDataService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new UserProfileModel
            {
                Email = await _userManager.GetEmailAsync(user),
                Id = await _userManager.GetUserIdAsync(user),
                Username = await _userManager.GetUserNameAsync(user)
            };

            var image = await _imageDataService.GetUserImage(user);

            if(image != null) model.ImageId = image.Id.ToString();

            return View(model);
        }

        [HttpGet]
        public IActionResult Password(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(UserPasswordInputModel inputModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (inputModel.CurrentPassword == null)
            {
                ModelState.AddModelError(nameof(inputModel.CurrentPassword), "Invalid Password");
                return View(inputModel);
            }

            var currentPasswordExists = await _userManager.CheckPasswordAsync(user, inputModel.CurrentPassword);
            if (!currentPasswordExists)
            {
                ModelState.AddModelError(nameof(inputModel.CurrentPassword), "Password does not match your current one");
                return View(inputModel);
            }

            await _userManager.ChangePasswordAsync(user, inputModel.CurrentPassword, inputModel.NewPassword);

            return View("~/Views/User/Success.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileModel inputModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(inputModel.Username == null || inputModel.Username.Length < 2)
            {
                ModelState.AddModelError(nameof(inputModel.Username), "Invalid Username");

                return View(inputModel);
            }
            if (!inputModel.Email.Contains("@") || inputModel.Email == null)
            {
                ModelState.AddModelError(nameof(inputModel.Email), "Invalid Email");

                return View(inputModel);
            }

            user.Email = inputModel.Email;
            user.UserName = inputModel.Username;

            await _userManager.UpdateAsync(user);

            var image = new ImageInputModel
            {
                Content = inputModel.Image.OpenReadStream(),
                Name = inputModel.Image.FileName,
                Type = inputModel.Image.ContentType,
                UserId = user.Id
            };

            var updatedImage = await _imageDataService.UpdateForUser(image);
            
            TempData[TempDataConstants.UserImageId] = updatedImage.Id.ToString();

            return RedirectToAction(nameof(Profile));
        }
    }
}

