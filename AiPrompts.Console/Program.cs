using AiPrompts.LMStudioLibrary;
using System.CodeDom;

namespace AiPrompts
{
    // "mistral-small-24b-instruct-2501"
    // "phi-4"
    // "qwen2.5-coder-32b-instruct"
    // "deepseek-r1-distill-llama-8b"
    public static class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> tokens
                = LMStudioConnection
                    .FetchAiReplies(
                        endpoint: "http://192.168.1.7:1232",
                        aiModel: "deepseek-r1-distill-llama-8b",
                        message: "Using C# write web service."
                    );

            ConsoleColor InitalBackgoundColor = Console.BackgroundColor;
            try
            {

                long index = 0;
                foreach (string token in tokens)
                {
                    WriteToken(index, token);
                    index++;
                }
            }
            finally
            {
                Console.BackgroundColor = InitalBackgoundColor;
            }
        }


        private static void WriteToken(long index, string token)
        {
            bool oddNumber = (index % 2 == 0);
            if (oddNumber)
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            else
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(token);
        }
    }
}
