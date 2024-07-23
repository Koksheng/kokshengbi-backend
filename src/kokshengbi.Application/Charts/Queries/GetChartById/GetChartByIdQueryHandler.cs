using AutoMapper;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using MediatR;

namespace kokshengbi.Application.Charts.Queries.GetChartById
{
    public class GetChartByIdQueryHandler :
        IRequestHandler<GetChartByIdQuery, ChartSafetyResult>
    {
        private readonly IChartRepository _chartRepository;
        private readonly IMapper _mapper;

        public GetChartByIdQueryHandler(IChartRepository chartRepository, IMapper mapper)
        {
            _chartRepository = chartRepository;
            _mapper = mapper;
        }

        public async Task<ChartSafetyResult> Handle(GetChartByIdQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            var chart = await _chartRepository.GetById(query.Id);
            if (chart == null)
            {
                throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found.");
            }
            var result = _mapper.Map<ChartSafetyResult>(chart);

            return result;
        }
    }
}
