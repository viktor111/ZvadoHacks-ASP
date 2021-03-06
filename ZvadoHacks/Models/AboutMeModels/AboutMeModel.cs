using System;
using System.Collections.Generic;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Models.AboutMeModels
{
    public class AboutMeModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string GitHubLink { get; set; }

        public string LinkedInLink { get; set; }

        public string Description { get; set; }

        public List<ProjectData> Projects { get; set; }
    }
}
