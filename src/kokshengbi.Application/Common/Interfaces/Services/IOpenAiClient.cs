namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IOpenAiClient
    {
        Task<string> GenerateTextAsync(string prompt);
    }
}
