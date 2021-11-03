using Microsoft.AspNetCore.Http;

namespace ZvadoHacks.Models.UserModels
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
