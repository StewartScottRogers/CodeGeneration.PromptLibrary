using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AiPrompts.LMStudio.Extentions
{
    public static class GetModelsAsyncExtensions
    {
        public static async Task<string> GetModelsAsync(this LMStudioSingleton lmStudioSingletonInstance, string baseUrl = "http://localhost:1234")
        {
            HttpResponseMessage httpResponseMessage
                = await lmStudioSingletonInstance
                    .HttpClient.GetAsync($"{baseUrl}/v1/models");

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new ApplicationException($"API request failed: {httpResponseMessage.StatusCode}");

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}

