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
            var image = await DbContext.Images
                .FirstOrDefaultAsync(i => i.ArticleId == article.Id);

            var comments = await DbContext.Comments
                    .Where(c => c.ArticleId == article.Id)
                    .ToListAsync();

            DbContext.Remove(image);
            DbContext.RemoveRange(comments);
            DbContext.Remove(article);

            await DbContext.SaveChangesAsync();

            return article;
        }

        public override async Task<Article> Get(Guid id)
        {
            var result = await DbContext.Articles
                .FirstOrDefaultAsync(a => a.Id == id);

            var image = await DbContext.Images
                .FirstOrDefaultAsync(i => i.ArticleId == result.Id);

            var comments = await DbContext.Comments
                .Where(c => c.ArticleId == id)
                .Include(c => c.User)
                .ToListAsync();

            result.Image = image;
            result.Comments = comments;

            return result;
        }

        public override async Task<IEnumerable<Article>> All()
        {
            var articles = await DbContext.Articles
                .ToListAsync();

            var result = new List<Article>();

            for (int i = 0; i < articles.Count; i++)
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

                var commentsForArticle = await DbContext.Comments
                    .Where(c => c.ArticleId == currentArticle.Id)
                    .ToListAsync();

                articleToAdd.Comments = commentsForArticle;

                var imageForArticle = await DbContext.Images
                    .FirstOrDefaultAsync(imageData => imageData.ArticleId == currentArticle.Id);

                articleToAdd.Image = imageForArticle;

                result.Add(articleToAdd);
            }

            return result;
        }
    }
}
