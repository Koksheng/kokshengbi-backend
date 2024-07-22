using kokshengbi.Domain.ChartAggregate;
using kokshengbi.Domain.Common.Models;
using kokshengbi.Domain.UserAggregate;
using kokshengbi.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace kokshengbi.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
        public DataContext(DbContextOptions<DataContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
        {
            _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Chart> Charts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Ignore<List<IDomainEvent>>()
                .ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
