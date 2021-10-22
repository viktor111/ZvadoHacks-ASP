using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data.Repositories
{
    public class ProjectDataRepository : GenericRepository<ProjectData>
    {
        public ProjectDataRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    
    }
}
