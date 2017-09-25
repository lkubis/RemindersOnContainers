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

        public static class Reminders
        {
            public static string Items(string baseUri, int pageSize = 10, int pageIndex = 0)
            {
                return $"{baseUri}/items?pageSize={pageSize}&pageIndex={pageIndex}";
            }
        }
    }
}