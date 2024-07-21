using kokshengbi.Domain.Common.Models;
using kokshengbi.Domain.UserAggregate.Events;
using kokshengbi.Domain.UserAggregate.ValueObjects;

namespace kokshengbi.Domain.UserAggregate
{
    public sealed class User : AggregateRoot<UserId, int>
    {
        public string userName { get; set; }
        public string userAccount { get; set; }
        public string userAvatar { get; set; }
        public int? gender { get; set; }
        public string userRole { get; set; }
        public string userPassword { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public bool? isDelete { get; set; }

        private User(
            UserId userId,
            string userName,
            string userAccount,
            string userAvatar,
            int? gender,
            string userRole,
            string userPassword,
            DateTime createTime,
            DateTime updateTime,
            bool? isDelete)
            : base(userId)
        {
            userName = userName;
            userAccount = userAccount;
            userAvatar = userAvatar;
            gender = gender;
            userRole = userRole;
            userPassword = userPassword;
            createTime = createTime;
            updateTime = updateTime;
            isDelete = isDelete;
        }

        public static User Create(
            string userName,
            string userAccount,
            string userAvatar,
            int gender,
            string userRole,
            string userPassword)
        {
            var user = new User(
                null,  // EF Core will set this value
                userName,
                userAccount,
                userAvatar,
                gender,
                userRole,
                userPassword,
                DateTime.UtcNow,
                DateTime.UtcNow,
                false);
            user.AddDomainEvent(new UserCreated(user));
            return user;
        }

        // Private parameterless constructor for EF Core
        private User() : base(null)
        {
            // EF Core requires an empty constructor for materialization
        }
    }
}
