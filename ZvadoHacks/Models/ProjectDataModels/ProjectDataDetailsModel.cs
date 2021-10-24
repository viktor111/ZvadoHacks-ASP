using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models.ProjectDataModels
{
    public class ProjectDataDetailsModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CodeLink { get; set; }

        public string DeployedLink { get; set; }
    }
}
