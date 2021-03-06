using System;

namespace ZvadoHacks.Models.ArticleModels
{
    public class ArticlePreviewModel
    {
        public string ArticleId { get; set; }

        public string PreviewImageId { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PreviewContent { get; set; }

        public string Content { get; set; }
    }
}
