using kokshengbi.Contracts.Chart;

namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IYuCongMingClient
    {
        Task<string> DoChatAsync(GenChartByAiDevChatRequest devChatRequest);
    }
}
