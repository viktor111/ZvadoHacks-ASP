using System;

namespace ZvadoHacks.Models.ProjectDataModels
{
    public class ProjectDataUpdateModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CodeLink { get; set; }

        public string DeployedLink { get; set; }
    }
}
