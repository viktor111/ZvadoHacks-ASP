using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data.Repositories
{
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
