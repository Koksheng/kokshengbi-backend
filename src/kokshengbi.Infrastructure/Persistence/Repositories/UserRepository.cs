using Microsoft.EntityFrameworkCore;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Models;
using kokshengbi.Domain.UserAggregate;
using kokshengbi.Domain.UserAggregate.Events;
using kokshengbi.Domain.UserAggregate.ValueObjects;

namespace kokshengbi.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByUserAccount(string userAccount)
        {
            var user = await _context.Users
                             .Where(u => u.isDelete != true && u.userAccount == userAccount)
                             .FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> CreateUser(User user)
        {
            // Check if user already exists
            var newUser = await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync(); // suppose can add Domain Event here, but since UserId is null, cant add Domain Event here

            // Add domain event after the user is saved and the ID is generated
            user.AddDomainEvent(new UserCreated(user));

            // Save changes again to persist domain event
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<User> GetUser(int id)
        {
            // Create a UserId object from the provided integer ID
            var userId = UserId.Create(id);

            // Query the database for the user with the specified ID
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<int> Update(User user)
        {
            // Attach the entity to the context and mark it as modified
            _context.Users.Update(user);
            // Save the changes to the database
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<User>> ListByPage(User query, int current, int pageSize, string sortField, string sortOrder)
        {
            var queryable = _context.Users.AsQueryable();

            if (query.Id != null)
            {
                queryable = queryable.Where(i => i.Id == query.Id);
            }

            if (!string.IsNullOrEmpty(query.userName))
            {
                queryable = queryable.Where(i => i.userName.Contains(query.userName));
            }

            if (!string.IsNullOrEmpty(query.userAccount))
            {
                queryable = queryable.Where(i => i.userAccount.Contains(query.userAccount));
            }

            if (query.gender != null)
            {
                queryable = queryable.Where(i => i.gender == query.gender);
            }

            if (!string.IsNullOrEmpty(query.userRole))
            {
                queryable = queryable.Where(i => i.userRole.Contains(query.userRole));
            }

            if (query.isDelete != null)
            {
                queryable = queryable.Where(i => i.isDelete == query.isDelete);
            }

            // Continue with other filters...

            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortOrder == "asc")
                {
                    queryable = queryable.OrderBy(e => EF.Property<object>(e, sortField));
                }
                else
                {
                    queryable = queryable.OrderByDescending(e => EF.Property<object>(e, sortField));
                }
            }

            var totalCount = await queryable.CountAsync();
            var items = await queryable.Skip((current - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<User>(items, totalCount, current, pageSize);
        }
    }
}
