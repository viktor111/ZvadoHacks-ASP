namespace ZvadoHacks.Helpers
{
    public static class ArticleContentTrimmer
    {
        const int AllowedCharsNum = 400;

        public static string Trim(string content)
        {
            if (content.Length <= AllowedCharsNum) return content;
            var str = content.Substring(0, AllowedCharsNum);
            var result = str.Substring(0, str.LastIndexOf(' '));

            return result;

        }
    }
}
