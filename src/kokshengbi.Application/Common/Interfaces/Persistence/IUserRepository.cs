using kokshengbi.Application.Common.Models;
using kokshengbi.Domain.UserAggregate;

namespace kokshengbi.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserAccount(string userAccount);
        Task<int> CreateUser(User user);
        Task<User> GetUser(int id);
        Task<int> Update(User user);
        Task<PaginatedList<User>> ListByPage(User query, int current, int pageSize, string sortField, string sortOrder);
    }
}
