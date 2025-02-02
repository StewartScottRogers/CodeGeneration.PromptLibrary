using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiPrompts.LMStudioLibrary
{
    public static class LMStudioConnection
    {
        public static IEnumerable<string> FetchAiReplies(string endpoint, string message)
        {

            OpenAIClient openAIClient
               = new OpenAIClient(
                   new ApiKeyCredential("api-key"),
                   new OpenAIClientOptions { Endpoint = new Uri(endpoint), }
               );

            MiddlewareStreamingAgent<OpenAIChatAgent> openAIChatAgent =
                new OpenAIChatAgent(
                    name: "Software Engineer",
                    systemMessage: "You Write Code.",
                    chatClient: openAIClient
                                    .GetChatClient("phi-4")
                                                //("qwen2.5-coder-32b-instruct") 
                                                //("deepseek-r1-distill-llama-8b")
                )
                .RegisterMessageConnector();

            IAsyncEnumerable<IMessage> tokens
                = openAIChatAgent
                    .GenerateStreamingReplyAsync(new[] { new TextMessage(Role.User, message) });


            foreach (IMessage token in tokens.ToBlockingEnumerable())
            {
                string messageContent
                    = token
                        .GetContent();

                yield return messageContent;
            }
        }
    }
}