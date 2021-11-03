using System;

namespace ZvadoHacks.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public Article Article { get; set; }

        public Guid ArticleId { get; set; }
    }
}
