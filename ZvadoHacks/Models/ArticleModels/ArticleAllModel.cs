using X.PagedList;

namespace ZvadoHacks.Models.ArticleModels
{
    public class ArticleAllModel
    {
        public IPagedList<ArticlePreviewModel> Articles { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}