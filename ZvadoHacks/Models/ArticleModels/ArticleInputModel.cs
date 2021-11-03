using Microsoft.AspNetCore.Http;

namespace ZvadoHacks.Models.ArticleModels
{
    public class ArticleInputModel
    {
        public IFormFile Image { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
    }
}
