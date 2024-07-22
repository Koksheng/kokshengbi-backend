using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Users.Common;
using kokshengbi.Contracts.Common;
using MediatR;

namespace kokshengbi.Application.Users.Queries.ListUserByPage
{
    public class ListUserByPageQuery : PageRequest, IRequest<PaginatedList<UserSafetyResult>>
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public int? Gender { get; set; }
        public string UserRole { get; set; }
        public int? IsDelete { get; set; }
        public ListUserByPageQuery() { }
        public ListUserByPageQuery(
            int id, string userName, string userAccount,
            int? gender, string userRole, int? isDelete, int current, int pageSize,
            string sortField, string sortOrder
            )
        {
            Id = id;
            UserName = userName;
            UserAccount = userAccount;
            Gender = gender;
            UserRole = userRole;
            IsDelete = isDelete;
            Current = current;
            PageSize = pageSize;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }
}
