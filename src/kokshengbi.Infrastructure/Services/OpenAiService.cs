using kokshengbi.Application.Common.Interfaces.Services;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;

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
            var chatRequest = new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,  // This corresponds to gpt-3.5-turbo, json *ChatGPTTurbo_1106)
                //Model = "gpt-3.5-turbo",
                //Model = "davinci-002",
                Messages = new[]
            {
                new ChatMessage(ChatMessageRole.System, "You are a helpful assistant designed to output JSON."),
                new ChatMessage(ChatMessageRole.User, prompt)
            },
                MaxTokens = 450,
                Temperature = 0.3
            };

            var response = await _openAiApi.Chat.CreateChatCompletionAsync(chatRequest);

            if (response != null && response.Choices != null && response.Choices.Count > 0)
            {
                return response.Choices[0].Message.Content;
            }
            else
            {
                throw new Exception("No response received from the API.");
            }
        }
    }
}
