using System.Net.Http;

namespace AiPrompts
{
    public static class LMStudioSingleton
    {
        public static HttpClient HttpClient { get; private set; }

        static LMStudioSingleton()
        {
            HttpClient = new HttpClient();
        }
    }
}
