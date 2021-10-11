using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models
{
    public class ArticleDetailsModel
    {
        public string ArticleId { get; set; }

        public string ImageId { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }
    }
}
