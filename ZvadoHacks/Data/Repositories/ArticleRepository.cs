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

        public override async Task<Article> Delete(Article article)
        {
            var image = await _dbContext.Images
                .FirstOrDefaultAsync(i => i.ArticleId == article.Id);

            var coments = await _dbContext.Comments
                    .Where(c => c.ArticleId == article.Id)
                    .ToListAsync();

            _dbContext.Remove(image);
            _dbContext.RemoveRange(coments);
            _dbContext.Remove(article);

            await _dbContext.SaveChangesAsync();

            return article;
        }

        public override async Task<Article> Get(Guid id)
        {
            var result = await _dbContext.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            var image = await _dbContext.Images
                .FirstOrDefaultAsync(i => i.ArticleId == result.Id);

            var coments = await _dbContext.Comments
                    .Where(c => c.ArticleId == id)
                    .Include(c => c.User)
                    .ToListAsync();

            result.Image = image;
            result.Comments = coments;

            return result;
        }

        public override async Task<IEnumerable<Article>> All()
        {
            var articles = await _dbContext.Articles
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

                var comentsForArticle = await _dbContext.Comments
                    .Where(c => c.ArticleId == currentArticle.Id)
                    .ToListAsync();

                articleToAdd.Comments = comentsForArticle;

                var imageForArticle = await _dbContext.Images
                    .FirstOrDefaultAsync(i => i.ArticleId == currentArticle.Id);

                articleToAdd.Image = imageForArticle;

                result.Add(articleToAdd);
            }

            return result;
        }
    }
}
