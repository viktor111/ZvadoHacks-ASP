using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models.User
{
    public class UserProfileModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IFormFile Image { get; set; }

        public string ImageId { get; set; }
    }
}
