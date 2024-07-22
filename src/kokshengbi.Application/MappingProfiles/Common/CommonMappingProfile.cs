using AutoMapper;
using kokshengbi.Application.Common.Models;

namespace kokshengbi.Application.MappingProfiles.Common
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            // Mapping for PaginatedList<T>
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>)).ConvertUsing(typeof(PaginatedListTypeConverter<,>));
        }
    }

    // Generic type converter for PaginatedList<T>
    public class PaginatedListTypeConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
        {
            var items = context.Mapper.Map<List<TDestination>>(source.Items);
            return new PaginatedList<TDestination>(items, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
