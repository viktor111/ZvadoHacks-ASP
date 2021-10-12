using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
