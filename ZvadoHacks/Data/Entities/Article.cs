using System;
using System.Collections.Generic;

namespace ZvadoHacks.Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int ViewsCount { get; set; }

        public ImageData Image { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
