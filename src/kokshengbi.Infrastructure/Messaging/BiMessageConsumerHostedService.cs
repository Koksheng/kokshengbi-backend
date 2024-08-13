using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace kokshengbi.Infrastructure.Messaging
{
    public class BiMessageConsumerHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BiMessageConsumerHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start consuming messages
            using (var scope = _scopeFactory.CreateScope())
            {
                var biMessageConsumer = scope.ServiceProvider.GetRequiredService<IBiMessageConsumer>();
                biMessageConsumer.StartConsuming();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Handle cleanup if necessary
            return Task.CompletedTask;
        }
    }
}
