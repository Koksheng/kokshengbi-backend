using kokshengbi.Application.Charts.Common;
using MediatR;

namespace kokshengbi.Application.Charts.Queries.GetChartById
{
    public class GetChartByIdQuery : IRequest<ChartSafetyResult>
    {
        public int Id { get; }
        public string UserState { get; }

        public GetChartByIdQuery(int id, string userState)
        {
            Id = id;
            UserState = userState;
        }
    }
}
