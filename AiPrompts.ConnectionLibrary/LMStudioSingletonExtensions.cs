using System.Text;
using System.Text.Json;

namespace AiPrompts.ConnectionLibrary
{
    public static class LMStudioSingletonExtensions
    {
        public static async Task<string> SendChatRequestAsync(this LMStudioSingleton lmStudioSingletonInstance, string prompt, Action<string> onMessageReceived, string baseUrl = "http://localhost:1234", string model = "default")
        {
            var requestBodyTuple = new
            {
                model = model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            StringContent jsonRequestBodyStringContent
                = new StringContent(
                    JsonSerializer.Serialize(requestBodyTuple),
                    Encoding.UTF8,
                    "application/json"
                );

            using var httpResponseMessage
                = await lmStudioSingletonInstance
                    .HttpClient
                        .PostAsync(
                            $"{baseUrl}/v1/chat/completions",
                            jsonRequestBodyStringContent
                        );

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception($"API request failed: {httpResponseMessage.StatusCode}");

            using Stream stream
                = await httpResponseMessage.Content.ReadAsStreamAsync();

            using StreamReader streamReader
                = new StreamReader(stream);

            StringBuilder stringBuilder = new StringBuilder();

            while (!streamReader.EndOfStream)
            {
                string? line = await streamReader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    stringBuilder.AppendLine(line);
                    onMessageReceived(line);
                }
            }

            return stringBuilder.ToString();
        }

        public static async Task<string> GetModelsAsync(this LMStudioSingleton lmStudioSingletonInstance, string baseUrl = "http://localhost:1234")
        {
            HttpResponseMessage httpResponseMessage
                = await lmStudioSingletonInstance
                    .HttpClient.GetAsync($"{baseUrl}/v1/models");

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception($"API request failed: {httpResponseMessage.StatusCode}");

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}

