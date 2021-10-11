using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data.Repositories
{
    public class ArticleRepository : GenericRepository<Article>
    {
        public ArticleRepository(ApplicationDbContext dbContext)
            :base(dbContext)
        {
        }

        public override async Task<Article> Get(Guid id)
        {
            var result = await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id == id);

            var image = await _dbContext.Images.FirstOrDefaultAsync(i => i.ArticleId == result.Id);

            result.Image = image;

            return result;
        }

        public override async Task<IEnumerable<Article>> All()
        {
            var articles = await _dbContext
                .Articles
                .ToListAsync();

            var result = new List<Article>();

            for (int i = 0; i < articles.Count(); i++)
            { 
                var currentArticle = articles[i];

                var articleToAdd = new Article();
                articleToAdd.Id = currentArticle.Id;
                articleToAdd.Heading = currentArticle.Heading;
                articleToAdd.Author = currentArticle.Author;
                articleToAdd.Content = currentArticle.Content;
                articleToAdd.ViewsCount = currentArticle.ViewsCount;
                articleToAdd.UpdatedOn = currentArticle.UpdatedOn;
                articleToAdd.CreatedOn = currentArticle.CreatedOn;                          

                var imageForArticle = await _dbContext
                    .Images
                    .FirstOrDefaultAsync(i => i.ArticleId == currentArticle.Id);

                articleToAdd.Image = imageForArticle;

                result.Add(articleToAdd);
            }

            return result;
        }
    }
}
