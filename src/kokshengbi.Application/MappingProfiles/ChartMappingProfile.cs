using AutoMapper;
using kokshengbi.Application.Charts.Commands.CreateChart;
using kokshengbi.Application.Charts.Commands.DeleteChart;
using kokshengbi.Application.Charts.Commands.UpdateChart;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Contracts.Chart;
using kokshengbi.Contracts.Common;
using kokshengbi.Domain.ChartAggregate;
using kokshengbi.Domain.ChartAggregate.ValueObjects;
using kokshengbi.Application.Charts.Queries.ListChartByPage;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.MappingProfiles.Common;

namespace kokshengbi.Application.MappingProfiles
{
    public class ChartMappingProfile : Profile
    {
        public ChartMappingProfile() 
        {
            // !!!!!!!!!!!!!! Type conversion configuration for ChartId to int !!!!!!!!!!!!!!
            CreateMap<ChartId, int>()
                .ConvertUsing(src => src.Value);

            // Create
            CreateMap<CreateChartRequest, CreateChartCommand>()
                .ForCtorParam("userState", opt => opt.MapFrom(src => string.Empty));
            CreateMap<CreateChartCommand, Chart>();

            // Delete
            CreateMap<DeleteRequest, DeleteChartCommand>()
                .ForCtorParam("userState", opt => opt.MapFrom(src => string.Empty));

            // Update
            CreateMap<UpdateChartRequest, UpdateChartCommand>()
                .ForCtorParam("userState", opt => opt.MapFrom(src => string.Empty));
            CreateMap<UpdateChartCommand, Chart>();

            // Get Chart By Id
            CreateMap<Chart, ChartSafetyResult>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.Value));
            CreateMap<ChartSafetyResult, ChartSafetyResponse>();

            // List Chart By Page
            CreateMap<QueryChartRequest, ListChartByPageQuery>()
               .ForMember(dest => dest.Current, opt => opt.MapFrom(src => src.current))
               .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.pageSize))
               .ForMember(dest => dest.SortField, opt => opt.MapFrom(src => src.sortField))
               .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.sortOrder));
            CreateMap<ListChartByPageQuery, Chart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id > 0 ? ChartId.Create(src.Id) : null)); // Adjust based on how ChartId is created

            // Mapping for PaginatedList<ChartSafetyResult> to PaginatedList<ChartSafetyResponse>
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>)).ConvertUsing(typeof(PaginatedListTypeConverter<,>));

        }
    }
}
