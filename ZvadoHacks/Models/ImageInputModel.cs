using System;
using System.IO;

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
