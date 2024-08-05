using kokshengbi.Application.Common.Interfaces.Services;
using System.Collections.Concurrent;

namespace kokshengbi.Infrastructure.Services
{
    public class ThreadPoolService : IThreadPoolService
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public ThreadPoolService()
        {
            Task.Run(RunWorkerAsync);
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
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
                        await workItem(CancellationToken.None);
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
