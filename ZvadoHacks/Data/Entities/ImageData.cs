using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Data.Entities
{
    public class ImageData
    {
        public ImageData()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public string OriginalFileName { get; set; }

        public string OriginalType { get; set; }

        public byte[] OriginalContent { get; set; }

        public byte[] ThumbnailContent { get; set; }

        public byte[] ArticleFullscreenContent { get; set; }

        public byte[] ArticlePreviewContent { get; set; }

        public Article Article { get; set; }

        public Guid? ArticleId { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public Guid? ProjectDataId { get; set; }

        public ProjectData Project { get; set; }
    }
}
