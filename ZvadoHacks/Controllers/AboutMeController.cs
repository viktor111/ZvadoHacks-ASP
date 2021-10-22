using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Repositories;

namespace ZvadoHacks.Controllers
{
    public class AboutMeController : Controller
    {
        private readonly ProjectDataRepository _projectDataRepository;

        public AboutMeController(ProjectDataRepository projectDataRepository)
        {
            _projectDataRepository = projectDataRepository;
        }
    }
}
