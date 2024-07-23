using AutoMapper;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Models;
using kokshengbi.Domain.ChartAggregate;
using MediatR;

namespace kokshengbi.Application.Charts.Queries.ListChartByPage
{
    public class ListChartByPageQueryHandler :
        IRequestHandler<ListChartByPageQuery, PaginatedList<ChartSafetyResult>>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IMapper _mapper;

        public ListChartByPageQueryHandler(IChartRepository chartRepository, IMapper mapper)
        {
            _chartRepository = chartRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ChartSafetyResult>> Handle(ListChartByPageQuery query, CancellationToken cancellationToken)
        {
            // Apply default values if necessary
            query.ApplyDefaults();

            Chart chart = _mapper.Map<Chart>(query);
            chart.isDelete = 0;

            var paginatedResult = await _chartRepository.ListByPage(
                chart,
                query.Current.Value,
                query.PageSize.Value,
                query.SortField,
                query.SortOrder);

            var result = _mapper.Map<List<ChartSafetyResult>>(paginatedResult.Items);

            return new PaginatedList<ChartSafetyResult>(result, paginatedResult.TotalCount, query.Current.Value, query.PageSize.Value);
        }
    }
}
