using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data.Repositories
{
    public class AboutMeRepository : GenericRepository<AboutMe>
    {
        public AboutMeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }    
    }
}
