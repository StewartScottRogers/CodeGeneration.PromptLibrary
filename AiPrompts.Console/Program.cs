using AiPrompts.LMStudioLibrary;

namespace AiPrompts
{
    public static class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> tokens
                = LMStudioConnection
                    .FetchAiReplies(
                        endpoint: "http://192.168.1.7:1232",
                        message: "Using C# write web service."
                    );

            foreach (string token in tokens)
                Console.Write(token);
        }
    }
}
