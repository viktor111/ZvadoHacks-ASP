using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Models.AboutMeModels;

namespace ZvadoHacks.Controllers
{
    public class AboutMeController : Controller
    {
        private readonly IRepository<ProjectData> _projectDataRepository;
        private readonly IRepository<AboutMe> _aboutMeRepository;

        public AboutMeController
            (
                IRepository<ProjectData> projectDataRepository,
                IRepository<AboutMe> aboutMeRepository
            )
        {
            _projectDataRepository = projectDataRepository;
            _aboutMeRepository = aboutMeRepository;
        }

        public async Task<IActionResult> AboutMe(string id)
        {
            var guid = new Guid(id);

            var aboutMeData = await _aboutMeRepository.Get(guid);
            var projects = await _projectDataRepository.All();

            var model = new AboutMeModel
            {
                Id = aboutMeData.Id,
                FullName = aboutMeData.FullName,
                Description = aboutMeData.Description,
                Email = aboutMeData.Email,
                GitHubLink = aboutMeData.GitHubLink,
                LinkedInLink = aboutMeData.LinkedInLink,
                PhoneNumber = aboutMeData.PhoneNumber,
                Projects = projects.ToList()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AboutMeInputModel inputModel)
        {
            var aboutMe = new AboutMe
            {
                FullName = inputModel.FullName,
                Description = inputModel.Description,
                Email = inputModel.Email,
                PhoneNumber = inputModel.PhoneNumber,
                GitHubLink = inputModel.GitHubLink,
                LinkedInLink = inputModel.LinkedInLink
            };

            var result = await _aboutMeRepository.Add(aboutMe);
                
            return RedirectToAction(nameof(AboutMe), new {id = result.Id.ToString()});
        }

        public async Task<IActionResult> Update(string id)
        {
            var guid = new Guid(id);

            var aboutMeData = await _aboutMeRepository.Get(guid);

            var model = new AboutMeUpdateModel
            {
                Id = aboutMeData.Id,
                FullName = aboutMeData.FullName,
                Description = aboutMeData.Description,
                Email = aboutMeData.Email,
                GitHubLink = aboutMeData.GitHubLink,
                LinkedInLink = aboutMeData.LinkedInLink,
                PhoneNumber = aboutMeData.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AboutMeInputModel inputModel)
        {
            var aboutMeData = await _aboutMeRepository.Get(inputModel.Id);

            aboutMeData.FullName = inputModel.FullName;
            aboutMeData.Description = inputModel.Description;
            aboutMeData.Email = inputModel.Email;
            aboutMeData.GitHubLink = inputModel.GitHubLink;
            aboutMeData.LinkedInLink = inputModel.LinkedInLink;
            aboutMeData.PhoneNumber = inputModel.PhoneNumber;

            await _aboutMeRepository.Update(aboutMeData);

            return RedirectToAction(nameof(AboutMe), inputModel.Id);
        }
    }
}
