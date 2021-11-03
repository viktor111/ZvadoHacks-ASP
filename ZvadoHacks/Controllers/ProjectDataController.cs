using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Models;
using ZvadoHacks.Models.ProjectDataModels;
using ZvadoHacks.Services.ImageService;

namespace ZvadoHacks.Controllers
{
    public class ProjectDataController : Controller
    {
        private readonly IRepository<ProjectData> _projectDataRepository;
        private readonly IImageProcessorService _imageProcessor;

        public ProjectDataController
            (
                IRepository<ProjectData> projectDataRepository, 
                IImageProcessorService imageProcessor
            )
        {
            _projectDataRepository = projectDataRepository;
            _imageProcessor = imageProcessor;
        }

        public async Task<IActionResult> Details(string id)
        {
            var guid = new Guid(id);

            var projectData = await _projectDataRepository.Get(guid);

            var model = new ProjectDataDetailsModel
            {
                Id = projectData.Id,
                Name = projectData.Name,
                Description = projectData.Description,
                DeployedLink = projectData.DeployedLink,
                CodeLink = projectData.CodeLink,
                ImageId = projectData.Image.Id.ToString()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDataInputModel inputModel)
        {
            var project = new ProjectData
            {
                Description = inputModel.Description,
                Name = inputModel.Name,
                CodeLink = inputModel.CodeLink,
                DeployedLink = inputModel.DeployedLink
            };
            var result = await _projectDataRepository.Add(project);

            var imageInputModel = new ImageInputModel
            {
                Name = inputModel.Image.FileName,
                Type = inputModel.Image.ContentType,
                Content = inputModel.Image.OpenReadStream(),
                ProjectId = result.Id
            };

            await _imageProcessor.Process(imageInputModel);

            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var guid = new Guid(id);

            var projectData = await _projectDataRepository.Get(guid);

            var model = new ProjectDataUpdateModel
            {
                Id = projectData.Id,
                Name = projectData.Name,
                Description = projectData.Description,
                DeployedLink = projectData.DeployedLink,
                CodeLink = projectData.CodeLink
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProjectDataUpdateModel inputModel)
        {
            var projectData = await _projectDataRepository.Get(inputModel.Id);
            projectData.Name = inputModel.Name;
            projectData.Description = inputModel.Description;
            projectData.DeployedLink = inputModel.DeployedLink;
            projectData.CodeLink = inputModel.CodeLink;

            await _projectDataRepository.Update(projectData);

            return RedirectToAction(nameof(Details));
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            var guid = new Guid(id);

            var projectData = await _projectDataRepository.Get(guid);

            await _projectDataRepository.Delete(projectData);

            return Redirect("/");
        }
    }
}
