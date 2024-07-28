using kokshengbi.Application.Common.Interfaces.Services;
using OpenAI_API;
using OpenAI_API.Completions;

namespace kokshengbi.Infrastructure.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly OpenAIAPI _openAiApi;

        public OpenAiService(string apiKey)
        {
            _openAiApi = new OpenAIAPI(apiKey);
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            var completionRequest = new CompletionRequest
            {
                Model = "babbage-002",
                //Model = "davinci-002",
                //Model = OpenAI_API.Models.Model.DavinciText,
                Prompt = prompt,
                MaxTokens = 250, // Increased token limit to accommodate a detailed response
                Temperature = 0.5, // Adjust temperature to reduce randomness
                TopP = 1.0,
                FrequencyPenalty = 0.0,
                PresencePenalty = 0.0
            };

            var completionResult = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);

            return completionResult.Completions[0].Text;
        }
    }
}
