using kokshengbi.Contracts.Chart;
using Newtonsoft.Json;

namespace kokshengbi.Application.Common.Utils
{
    public static class OpenAiResponseParser
    {
        public static OpenAIApiResponse ParseOpenAiResponse(string openAiResponse)
        {
            try
            {
                if (openAiResponse.TrimStart().StartsWith("{"))
                {
                    // Assume JSON format
                    return JsonConvert.DeserializeObject<OpenAIApiResponse>(openAiResponse);
                }
                else
                {
                    // Assume custom format
                    var echartIndex = openAiResponse.IndexOf("Echart:");
                    var conclusionIndex = openAiResponse.IndexOf("Conclusion:");

                    if (echartIndex != -1 && conclusionIndex != -1)
                    {
                        var echart = openAiResponse.Substring(echartIndex + 7, conclusionIndex - echartIndex - 7).Trim();
                        var conclusion = openAiResponse.Substring(conclusionIndex + 11).Trim();

                        return new OpenAIApiResponse(echart, conclusion);
                    }
                }

                throw new FormatException("Unknown response format");
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing OpenAI response: " + ex.Message, ex);
            }
        }
    }
}
