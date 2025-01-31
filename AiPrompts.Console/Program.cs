using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace AiPrompts.Console
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string executingAssemblyLocation
                = Assembly.GetExecutingAssembly().Location;

            string currentDirectoryPath
                = Path.GetDirectoryName(executingAssemblyLocation) ?? throw new Exception("The ExecutingAssembly Location was not found.");

            IConfigurationRoot configurationRoot
                = new ConfigurationBuilder()
                    .SetBasePath(currentDirectoryPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .Build();

            string openAIKey
                = configurationRoot
                    .GetSection("AppSettings:OPENAI_API_KEY")?.Value ?? throw new Exception("Please set OPENAI_API_KEY environment variable.");

            string baseURL
                = configurationRoot
                    .GetSection("AppSettings:BASE_URL")?.Value ?? throw new Exception("Please set BASE_URL environment variable.");


            Debug.WriteLine($"{nameof(openAIKey)}: '{openAIKey}'.");

            LMStudioConnection.ConnectConsole(openAIKey).Wait();
        }
    }
}
