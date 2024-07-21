using kokshengbi.Domain.Common.Models;

namespace kokshengbi.Domain.UserAggregate.Events
{
    public record UserCreated(User User) : IDomainEvent;
}
