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
                Prompt = prompt,
                MaxTokens = 100
            };

            var completionResult = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);

            return completionResult.Completions[0].Text;
        }
    }
}
