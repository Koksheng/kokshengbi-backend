using AutoMapper;
using kokshengbi.Application.Charts.Commands.CreateChart;
using kokshengbi.Application.Charts.Commands.DeleteChart;
using kokshengbi.Application.Charts.Commands.UpdateChart;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Contracts.Chart;
using kokshengbi.Contracts.Common;
using kokshengbi.Domain.ChartAggregate;
using kokshengbi.Domain.ChartAggregate.ValueObjects;

namespace kokshengbi.Application.MappingProfiles
{
    public class ChartMappingProfile : Profile
    {
        public ChartMappingProfile() 
        {
            // !!!!!!!!!!!!!! Type conversion configuration for InterfaceInfoId to int !!!!!!!!!!!!!!
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
        }
    }
}
