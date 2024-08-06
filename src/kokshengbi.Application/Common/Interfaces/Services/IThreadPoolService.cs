using Microsoft.Extensions.DependencyInjection;

namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IThreadPoolService
    {
        //void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
        void QueueBackgroundWorkItem(Func<IServiceScope, CancellationToken, Task> workItem);
    }
}
