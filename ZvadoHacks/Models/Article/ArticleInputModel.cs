﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Models.Article
{
    public class ArticleInputModel
    {
        public IFormFile Image { get; set; }

        public string Heading { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
    }
}