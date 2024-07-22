using AutoMapper;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Users.Common;
using kokshengbi.Domain.UserAggregate;
using MediatR;

namespace kokshengbi.Application.Users.Queries.ListUserByPage
{
    public class ListUserByPageQueryHandler :
        IRequestHandler<ListUserByPageQuery, PaginatedList<UserSafetyResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ListUserByPageQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserSafetyResult>> Handle(ListUserByPageQuery query, CancellationToken cancellationToken)
        {
            // Apply default values if necessary
            query.ApplyDefaults();

            User user = _mapper.Map<User>(query);
            //user.isDelete = false;

            var paginatedResult = await _userRepository.ListByPage(
                user,
                query.Current.Value,
                query.PageSize.Value,
                query.SortField,
                query.SortOrder);

            var result = _mapper.Map<List<UserSafetyResult>>(paginatedResult.Items);

            return new PaginatedList<UserSafetyResult>(result, paginatedResult.TotalCount, query.Current.Value, query.PageSize.Value);
        }
    }
}
