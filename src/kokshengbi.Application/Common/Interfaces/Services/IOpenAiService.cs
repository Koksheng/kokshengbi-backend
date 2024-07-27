namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IOpenAiService
    {
        Task<string> GenerateTextAsync(string prompt);
    }
}
