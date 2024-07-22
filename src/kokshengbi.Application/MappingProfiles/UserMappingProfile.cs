using AutoMapper;
using kokshengbi.Application.Users.Commands.Register;
using kokshengbi.Application.Users.Commands.UpdateUser;
using kokshengbi.Application.Users.Common;
using kokshengbi.Application.Users.Queries.Login;
using kokshengbi.Contracts.User;
using kokshengbi.Domain.UserAggregate;
using kokshengbi.Domain.UserAggregate.ValueObjects;

namespace kokshengbi.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // !!!!!!!!!!!!!! Type conversion configuration for UserId to int !!!!!!!!!!!!!!
            CreateMap<UserId, int>().ConvertUsing(src => src.Value);

            // UserController
            CreateMap<UserRegisterRequest, UserRegisterCommand>();
            CreateMap<UserLoginRequest, UserLoginQuery>();

            //UserRegisterCommandHandler
            CreateMap<UserRegisterCommand, User>();
            CreateMap<User, UserSafetyResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))  // Mapping UserId.Value to Id
                .ForCtorParam("token", opt => opt.MapFrom(src => string.Empty));
            CreateMap<UserSafetyResult, UserSafetyResponse>();

            //// List User By Page
            //CreateMap<QueryUserRequest, ListUserByPageQuery>()
            //   .ForMember(dest => dest.Current, opt => opt.MapFrom(src => src.current))
            //   .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.pageSize))
            //   .ForMember(dest => dest.SortField, opt => opt.MapFrom(src => src.sortField))
            //   .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.sortOrder));
            //CreateMap<ListUserByPageQuery, User>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id > 0 ? UserId.Create(src.Id) : null)); // Adjust based on how UserId is created
            //CreateMap<UserSafetyResult, AdminPageUserSafetyResponse>();

            // Update
            CreateMap<UpdateUserRequest, UpdateUserCommand>()
                .ForCtorParam("userState", opt => opt.MapFrom(src => string.Empty));
            CreateMap<UpdateUserCommand, User>();

            //// Mapping for PaginatedList<UserSafetyResult> to PaginatedList<UserSafetyResponse>
            //CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>)).ConvertUsing(typeof(PaginatedListTypeConverter<,>));

            //// Get User Access Key & Secret Key
            //CreateMap<UserSafetyResult, UserDevKeyResponse>();

        }
    }
}
