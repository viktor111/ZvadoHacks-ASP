using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models.CommentModels
{
    public class CommentModel
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public Guid ArticleId { get; set; }
    }
}
