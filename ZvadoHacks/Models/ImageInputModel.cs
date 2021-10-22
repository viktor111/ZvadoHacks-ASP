using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models
{
    public class ImageInputModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public Stream Content { get; set; }

        public Guid ArticleId { get; set; }

        public string UserId { get; set; }

        public Guid ProjectId { get; set; }
    }
}
