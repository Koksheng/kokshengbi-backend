using kokshengbi.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace kokshengbi.Infrastructure.Services
{
    public class ThreadPoolService : IThreadPoolService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<IServiceScope, CancellationToken, Task>>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public ThreadPoolService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Task.Run(RunWorkerAsync);
        }

        public void QueueBackgroundWorkItem(Func<IServiceScope, CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        private async Task RunWorkerAsync()
        {
            while (true)
            {
                await _signal.WaitAsync();

                if (_workItems.TryDequeue(out var workItem))
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        await workItem(scope, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                    }
                }
            }
        }
    }
}
