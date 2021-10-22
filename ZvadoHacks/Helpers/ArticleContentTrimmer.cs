using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZvadoHacks.Helpers
{
    public class ArticleContentTrimmer
    {
        const int allowedCharsNum = 400;

        public static string Trim(string content)
        {
            if (content.Length > allowedCharsNum)
            {
                var str = content.Substring(0, allowedCharsNum);
                var result = str.Substring(0, str.LastIndexOf(' '));

                return result;
            }

            return content;
        }
    }
}
