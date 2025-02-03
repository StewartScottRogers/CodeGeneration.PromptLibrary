using System;
using System.Threading.Tasks;

namespace AiPrompts
{
    // "mistral-small-24b-instruct-2501"
    // "phi-4"
    // "qwen2.5-coder-32b-instruct"
    // "deepseek-r1-distill-llama-8b"
    public static class Program
    {
        static async Task Main(string[] args)
        {
            string result
                = await LMStudioSingleton
                    .HttpClient
                        .SendChatRequestAsync(
                            prompt: "Use C# to write out the Fibonacci series.",
                            baseUrl: @"http://192.168.1.7:1232",
                            model: "deepseek-r1-distill-llama-8b"
                        );



            //TokenShuttle tokenShuttle
            //    = LMStudioConnection
            //        .FetchAiReplies(
            //            endpoint: "http://192.168.1.7:1232",
            //            aiModel: "qwen2.5-coder-32b-instruct",
            //            message: "Use C# to write out the Fibonacci series. "
            //        );

            //ConsoleColor InitalBackgoundColor = Console.BackgroundColor;
            //try
            //{

            //    long index = 0;
            //    foreach (string token in tokenShuttle)
            //    {
            //        WriteToken(index, token);
            //        index++;
            //    }
            //}
            //finally
            //{
            //    Console.BackgroundColor = InitalBackgoundColor;
            //}

            //Console.WriteLine();
            //Console.WriteLine(new string('-', 80));
            //string input = tokenShuttle.ToString();
            //Console.WriteLine(input);
            //Console.WriteLine();
            //Console.WriteLine(new string('-', 80));
            //string output = tokenShuttle.Compile();
            //Console.WriteLine();
            //Console.WriteLine(output);
            //Console.WriteLine();
            //Console.WriteLine(new string('-', 80));
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
