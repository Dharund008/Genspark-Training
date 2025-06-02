using System;
using Chat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Chat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ChatController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }
        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            //1.get api from config
            var api = _configuration["GeminiAI:ApiKey"];
            if (string.IsNullOrEmpty(api))
            {
                return StatusCode(500, "Gemini API key is missing.");
            }
            //2.setting up endpoint
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-001:generateContent?key={api}";
            ///gemini-1.5-flash
            //3.system prompt
            var systemPrompt = System.IO.File.ReadAllText("systemprompt.txt");

            //4.message prompt
            // var messages = new[]
            // {
            //     new { role = "system", content = systemPrompt },
            //     new { role = "user", content = request.Question }
            // };

            //5.req body:json obj.
            var fullInput = $"{systemPrompt.Trim()}\n\nUser: {request.Question}\nBot:";

            var requestData = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = fullInput }
                        }
                    }
                }
            };


            //  _httpClient.DefaultRequestHeaders.Authorization =
            //     new AuthenticationHeaderValue("Bearer", api);

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Gemini API Error: {error}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<ChatResponse>(responseString);

            if (parsed?.candidates == null || parsed.candidates.Count == 0)
            {
                return StatusCode(500, "No valid response from Gemini API.");
            }

            var reply = parsed.candidates[0].content?.parts?[0]?.text;

            return Ok(reply ?? "Gemini returned an empty response.");
        }
    }
}
