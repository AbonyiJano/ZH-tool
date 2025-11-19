using Microsoft.Extensions.Options;
using ZH_tool.Configurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ZH_tool.Services
{

    public class GeminiService : IGeminiService
    {
        private readonly GeminiSettings _geminiSettings;
        private readonly HttpClient _client;
        public GeminiService(IOptions<GeminiSettings> geminiOptions)
        {
            _client = new HttpClient();
            _geminiSettings = geminiOptions.Value;
        }
        public async Task<string> CallGeminiAPI(string prompt)
        {
            string apiKey = _geminiSettings.ApiKey;
            string model = _geminiSettings.ModelName;
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 8000
                }
            };

            string jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API hiba: {response.StatusCode}\n{responseBody}");
            }

            JObject jsonResponse = JObject.Parse(responseBody);
            string generatedText = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "";

            // JSON kinyerése a válaszból
            generatedText = generatedText.Trim();
            if (generatedText.StartsWith("```json"))
            {
                generatedText = generatedText.Substring(7);
            }
            if (generatedText.StartsWith("```"))
            {
                generatedText = generatedText.Substring(3);
            }
            if (generatedText.EndsWith("```"))
            {
                generatedText = generatedText.Substring(0, generatedText.Length - 3);
            }

            return generatedText.Trim();
        }

    }
}
