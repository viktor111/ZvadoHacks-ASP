using System;

namespace ZvadoHacks.Data.Entities
{
    public class AboutMe
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string GitHubLink { get; set; }

        public string LinkedInLink { get; set; }

        public string Description { get; set; }
    }
}
