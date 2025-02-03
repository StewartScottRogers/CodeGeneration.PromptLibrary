using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiPrompts
{
    public static class SendChatRequestAsyncExtension
    {
        public static async Task<string> SendChatRequestAsync(
            this HttpClient httpClient,
            string prompt,
            string baseUrl = "http://localhost:1234",
            string model = "default"
        )
        {
            var requestBodyTuple = new
            {
                model,
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
                = await httpClient
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
                string line = await streamReader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    stringBuilder.AppendLine(line);
                    //onMessageReceived(line);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
