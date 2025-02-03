using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AiPrompts
{
    public static class GetModelsAsyncExtensions
    {
        public static async Task<string> GetModelsAsync(this HttpClient httpClient, string baseUrl = "http://localhost:1234")
        {
            HttpResponseMessage httpResponseMessage
                = await httpClient
                    .GetAsync($"{baseUrl}/v1/models");

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new ApplicationException($"API request failed: {httpResponseMessage.StatusCode}");

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}

