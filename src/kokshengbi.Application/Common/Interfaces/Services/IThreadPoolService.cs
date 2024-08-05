namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IThreadPoolService
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
    }
}
