using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace AIRagChat.Services
{
    public class OpenAIEmbeddingProvider
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _url;

        public OpenAIEmbeddingProvider(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = Environment.GetEnvironmentVariable("OpenAI_Key_TranslateService") ?? "";
            _model = "text-embedding-3-small";
            _url = "https://api.openai.com/v1/embeddings";
        }

        public async Task<float[]> GenerateAsync(string text)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _url);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var request = new { model = _model, input = text };
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _http.SendAsync(httpRequest);
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            float[] vector = Array.Empty<float>();
            try
            {
                vector = doc.RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(x => x.GetSingle())
                .ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing embedding response: " + ex.Message);
            }
            return vector;
        }

        public string ToJson(float[] vector)
        {
            return JsonSerializer.Serialize(vector);
        }

        public float[] FromJson(string json)
        {
            return JsonSerializer.Deserialize<float[]>(json) ?? [];
        }
    }
}
