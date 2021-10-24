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

            var model = new AboutMeModel();
            model.Id = aboutMeData.Id;
            model.FullName = aboutMeData.FullName;
            model.Description = aboutMeData.Description;
            model.Email = aboutMeData.Email;
            model.GitHubLink = aboutMeData.GitHubLink;
            model.LinkedInLink = aboutMeData.LinkedInLink;
            model.PhoneNumber = aboutMeData.PhoneNumber;
            model.Projects = projects.ToList();

            return View(model);
        }

        public async Task<IActionResult> Update(string id)
        {
            var guid = new Guid(id);

            var aboutMeData = await _aboutMeRepository.Get(guid);

            var model = new AboutMeUpdateModel();
            model.Id = aboutMeData.Id;
            model.FullName = aboutMeData.FullName;
            model.Description = aboutMeData.Description;
            model.Email = aboutMeData.Email;
            model.GitHubLink = aboutMeData.GitHubLink;
            model.LinkedInLink = aboutMeData.LinkedInLink;
            model.PhoneNumber = aboutMeData.PhoneNumber;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AboutMeInputModel inputMopdel)
        {
            var aboutMeData = await _aboutMeRepository.Get(inputMopdel.Id);

            aboutMeData.FullName = inputMopdel.FullName;
            aboutMeData.Description = inputMopdel.Description;
            aboutMeData.Email = inputMopdel.Email;
            aboutMeData.GitHubLink = inputMopdel.GitHubLink;
            aboutMeData.LinkedInLink = inputMopdel.LinkedInLink;
            aboutMeData.PhoneNumber = inputMopdel.PhoneNumber;

            await _aboutMeRepository.Update(aboutMeData);

            return RedirectToAction(nameof(AboutMe), inputMopdel.Id);
        }
    }
}
