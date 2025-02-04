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
using System.Collections;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AiPrompts.LMStudioLibrary
{
    public static class LMStudioConnection
    {
        public static TokenShuttle FetchAiReplies(string endpoint, string aiModel, string message, string apiKey = "api-key-does-not-mater-for-lmstudio")
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



            MiddlewareAgent<MiddlewareStreamingAgent<OpenAIChatAgent>> dotnetInteractiveMiddlewareAgent =
                openAIChatAgent
                    .RegisterMiddleware(async (message, generateReplyOptions, innerAgent, cancellationToken) =>
                        {
                            IMessage lastMessage
                               = message.LastOrDefault();

                            if (lastMessage == null)
                                return await innerAgent.GenerateReplyAsync(message, generateReplyOptions, cancellationToken);

                            if (lastMessage.GetContent() is null)
                                return await innerAgent.GenerateReplyAsync(message, generateReplyOptions, cancellationToken);

                            string[] codeBlocks
                                = lastMessage
                                    .ExtractMessageCodeBlocks(@"```csharp", @"```")
                                        .ToArray();

                            if (codeBlocks.Length > 0)
                            {
                                CompositeKernel compositeKernel
                                   = DotnetInteractiveKernelBuilder
                                       .CreateDefaultInProcessKernelBuilder() // add C# and F# kernels
                                           .Build();

                                Collection<string> collection = new Collection<string>();

                                foreach (string codeBlock in codeBlocks)
                                {
                                    string result
                                        = await compositeKernel
                                            .RunSubmitCodeCommandAsync(
                                                codeBlock,
                                                "csharp",
                                                cancellationToken
                                            );

                                    collection.Add(result);
                                }


                                string results
                                    = string.Join('\n', collection);

                                return new TextMessage(Role.Assistant, results, from: openAIChatAgent.Name);
                            }

                            // no code block found, invoke next agent
                            return await innerAgent.GenerateReplyAsync(message, generateReplyOptions, cancellationToken);

                         }
                    );

            IEnumerable<string> tokens =
                openAIChatAgent
                    .GenerateStreamingReplyAsync(
                        new[] {
                            new TextMessage(Role.User, message)
                        }
                    ).ToBlockingEnumerable()
                        .Select(token => token.GetContent());

            return new TokenShuttle(
                tokens,
                dotnetInteractiveMiddlewareAgent
            );
        }

        public static IEnumerable<string> ExtractMessageCodeBlocks(this IMessage message, string codeBlockPrefix, string codeBlockSuffix)
        {
            string text = message.GetContent() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
                yield break;

            string pattern
                = (codeBlockPrefix + "([\\s\\S]*?)" + codeBlockSuffix)
                    .Trim();

            IEnumerable<string> matchCollection
                = Regex
                    .Matches(text, pattern)
                        .Select(
                            match =>
                                (match.Groups[0].Value)
                                    .TrimStart(codeBlockPrefix.ToCharArray())
                                        .TrimEnd(codeBlockSuffix.ToCharArray())
                        );

            foreach (string match in matchCollection)
                yield return match;
        }
    }


    public class TokenShuttle : IEnumerable<string>
    {
        private IEnumerable<string> Tokens;
        private MiddlewareAgent<MiddlewareStreamingAgent<OpenAIChatAgent>> DotnetInteractiveMiddlewareAgent;

        private StringBuilder StringBuilder = new StringBuilder();

        public TokenShuttle(
            IEnumerable<string> tokens,
            MiddlewareAgent<MiddlewareStreamingAgent<OpenAIChatAgent>> dotnetInteractiveMiddlewareAgent
        )
        {
            Tokens = tokens;
            DotnetInteractiveMiddlewareAgent = dotnetInteractiveMiddlewareAgent;
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var token in Tokens)
            {
                StringBuilder.Append(token.ToString());
                yield return token;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var token in Tokens)
            {
                StringBuilder.Append(token.ToString());
                yield return token;
            }
        }
        public string Compile()
        {
            string content = StringBuilder.ToString();

            IMessage message
                = DotnetInteractiveMiddlewareAgent
                    .SendAsync(new TextMessage(Role.User, content))
                        .Result;

            return message.GetContent();

        }
        override public string ToString()
        {
            return StringBuilder.ToString();
        }
    }
}