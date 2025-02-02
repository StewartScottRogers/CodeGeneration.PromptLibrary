using AutoGen.Core;
using AutoGen.DotnetInteractive;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using System.Linq;
using AutoGen.DotnetInteractive.Extension;

namespace AiPrompts.LMStudioLibrary
{
    public static class LMStudioConnection
    {
        public static IEnumerable<string> FetchAiReplies(string endpoint, string aiModel, string message, string apiKey = "api-key-does-not-mater-for-lmstudio")
        {
            OpenAIClient openAIClient
               = new OpenAIClient(
                   new ApiKeyCredential(apiKey),
                   new OpenAIClientOptions { Endpoint = new Uri(endpoint), }
               );

            MiddlewareStreamingAgent<OpenAIChatAgent> openAIChatAgent =
                new OpenAIChatAgent(
                    name: "Software Engineer",
                    systemMessage: "You Write Code.",
                    chatClient: openAIClient.GetChatClient(aiModel)
                ).RegisterMessageConnector();

            //CompositeKernel compositeKernel
            //   = DotnetInteractiveKernelBuilder
            //       .CreateDefaultInProcessKernelBuilder() // add C# and F# kernels
            //           .Build();

            //MiddlewareAgent<MiddlewareStreamingAgent<OpenAIChatAgent>> dotnetInteractiveMiddlewareAgent =
            //    openAIChatAgent
            //        .RegisterMiddleware(async (message, generateReplyOptions, innerAgent, cancellationToken) =>
            //            {
            //                IMessage lastMessage
            //                   = message.LastOrDefault();

            //                if (lastMessage == null || lastMessage.GetContent() is null)
            //                    return await innerAgent.GenerateReplyAsync(message, generateReplyOptions, cancellationToken);

            //                if (lastMessage.ExtractCodeBlock("```csharp", "```") is string codeSnippet)
            //                {
            //                    // execute code snippet
            //                    string result
            //                       = await compositeKernel.RunSubmitCodeCommandAsync(codeSnippet, "csharp");

            //                    return new TextMessage(Role.Assistant, result, from: openAIChatAgent.Name);
            //                }
            //                else
            //                {
            //                    // no code block found, invoke next agent
            //                    return await innerAgent.GenerateReplyAsync(message, generateReplyOptions, cancellationToken);
            //                }
            //            }
            //        );

            IEnumerable<string> tokens =
                openAIChatAgent
                    .GenerateStreamingReplyAsync(
                        new[] {
                            new TextMessage(Role.User, message)
                        }
                    ).ToBlockingEnumerable()
                        .Select(token => token.GetContent());

            return tokens;
        }
    }
}