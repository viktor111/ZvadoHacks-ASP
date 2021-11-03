using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data.Repositories
{
    public class ProjectDataRepository : GenericRepository<ProjectData>
    {
        public ProjectDataRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<ProjectData> Get(Guid id)
        {
            var result = await DbContext.ProjectsData.FirstOrDefaultAsync(x => x.Id == id);
            
            var image = await DbContext.Images.FirstOrDefaultAsync(x => x.ProjectDataId == id);
            
            result.Image = image;
            
            return result;
        }
    }
}
