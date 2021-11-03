using System;
using System.Collections.Generic;
using ZvadoHacks.Models.CommentModels;

namespace ZvadoHacks.Models.ArticleModels
{
    public class ArticleDetailsModel
    {
        public string ArticleId { get; set; }

        public string ImageId { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
