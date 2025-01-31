using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using OpenAI;
using System.Threading.Tasks;

namespace AiPrompts
{
    public static class LMStudioConnection
    {

        public static async Task ConnectConsole(string openAIKey, string model = "gpt-4o-mini")
        {
            //string openAIKey
            //    = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("Please set OPENAI_API_KEY environment variable.");

            var openAIClient
                = new OpenAIClient(openAIKey);

            MiddlewareStreamingAgent<OpenAIChatAgent> assistantAgent =
                new OpenAIChatAgent(
                    name: "assistant",
                    systemMessage: "You are an assistant that help user to do some tasks.",
                    chatClient: openAIClient.GetChatClient(model)
                )
                .RegisterMessageConnector()
                    .RegisterPrintMessage(); // register a hook to print message nicely to console


            // set human input mode to ALWAYS so that user always provide input
            MiddlewareAgent<UserProxyAgent> userProxyAgent 
                = new UserProxyAgent(
                    name: "user",
                    humanInputMode: HumanInputMode.ALWAYS
                )
                .RegisterPrintMessage();

            // start the conversation
            await userProxyAgent.InitiateChatAsync(
                receiver: assistantAgent,
                message: "Hey assistant, please do me a favor.",
                maxRound: 10
            );

        }


    }
}
