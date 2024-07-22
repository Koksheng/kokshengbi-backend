using kokshengbi.Application.Common.Interfaces.Services;

namespace kokshengbi.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
