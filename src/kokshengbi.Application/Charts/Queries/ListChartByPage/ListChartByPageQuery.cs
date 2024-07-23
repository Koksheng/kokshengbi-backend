using kokshengbi.Application.Charts.Common;
using kokshengbi.Application.Common.Models;
using kokshengbi.Contracts.Common;
using MediatR;

namespace kokshengbi.Application.Charts.Queries.ListChartByPage
{
    public class ListChartByPageQuery : PageRequest, IRequest<PaginatedList<ChartSafetyResult>>
    {

        public int Id { get; set; }
        public string Goal { get; set; }
        public string ChartName { get; set; }
        public string ChartType { get; set; }
        public int UserId { get; set; }
        public ListChartByPageQuery() { }
        public ListChartByPageQuery(
            int id, string goal, string chartName,
            string chartType, int userId, int current, int pageSize,
            string sortField, string sortOrder
            )
        {
            Id = id;
            Goal = goal;
            ChartName = chartName;
            ChartType = chartType;
            UserId = userId;
            Current = current;
            PageSize = pageSize;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }
}
