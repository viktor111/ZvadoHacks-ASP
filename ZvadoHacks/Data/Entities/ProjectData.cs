using System;

namespace ZvadoHacks.Data.Entities
{
    public class ProjectData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CodeLink { get; set; }

        public string DeployedLink { get; set; }

        public ImageData Image { get; set; }
    }
}
