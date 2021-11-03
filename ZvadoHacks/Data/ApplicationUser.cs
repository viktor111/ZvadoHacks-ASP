using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ZvadoHacks.Data.Entities;

namespace ZvadoHacks.Data
{
    public class ApplicationUser : IdentityUser
    {     
        public ImageData ProfilePicture { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
