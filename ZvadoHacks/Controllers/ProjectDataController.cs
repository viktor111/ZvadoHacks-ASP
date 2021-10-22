using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;
using ZvadoHacks.Data.Repositories;
using ZvadoHacks.Models;
using ZvadoHacks.Models.ProjectData;
using ZvadoHacks.Services;

namespace ZvadoHacks.Controllers
{
    public class ProjectDataController : Controller
    {
        private readonly ProjectDataRepository _projectDataRepository;
        private readonly IImageProcessorService _imageProcessor;

        public ProjectDataController
            (
                ProjectDataRepository projectDataRepository, 
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

            var model = new ProjectDataDetailsModel();
            model.Id = projectData.Id;
            model.Name = projectData.Name;
            model.Description = projectData.Description;
            model.DeployedLink = projectData.DeployedLink;
            model.CodeLink = projectData.CodeLink;

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDataInputModel inputModel)
        {
            var project = new ProjectData();
            project.Description = inputModel.Description;
            project.Name = inputModel.Name;
            project.CodeLink = inputModel.CodeLink;
            project.DeployedLink = inputModel.DeployedLink;
            var result = await _projectDataRepository.Add(project);

            var imageInpuModel = new ImageInputModel();
            imageInpuModel.Name = inputModel.Image.FileName;
            imageInpuModel.Type = inputModel.Image.ContentType;
            imageInpuModel.Content = inputModel.Image.OpenReadStream();
            imageInpuModel.ProjectId = result.Id;

            await _imageProcessor.Process(imageInpuModel);

            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var guid = new Guid(id);

            var projectData = await _projectDataRepository.Get(guid);

            var model = new ProjectDataUpdateModel();
            model.Id = projectData.Id;
            model.Name = projectData.Name;
            model.Description = projectData.Description;
            model.DeployedLink = projectData.DeployedLink;
            model.CodeLink = projectData.CodeLink;

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
    }
}
