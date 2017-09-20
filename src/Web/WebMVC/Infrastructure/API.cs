namespace WebMVC.Infrastructure
{
    public static class API
    {
        public static class Identity
        {
            public static string Token(string baseUri)
            {
                return $"{baseUri}/token";
            }
        }
    }
}