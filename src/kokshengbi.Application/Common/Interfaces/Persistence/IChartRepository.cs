using kokshengbi.Application.Common.Models;
using kokshengbi.Domain.ChartAggregate;

namespace kokshengbi.Application.Common.Interfaces.Persistence
{
    public interface IChartRepository
    {
        Task<int> Add(Chart chart);
        Task<Chart> GetById(int id);
        Task<int> DeleteById(int id);
        Task<int> Update(Chart chart);
        Task<PaginatedList<Chart>> ListByPage(Chart query, int current, int pageSize, string sortField, string sortOrder);
    }
}
