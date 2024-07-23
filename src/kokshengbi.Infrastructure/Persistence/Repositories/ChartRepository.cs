using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Domain.ChartAggregate;
using kokshengbi.Domain.ChartAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace kokshengbi.Infrastructure.Persistence.Repositories
{
    public class ChartRepository : IChartRepository
    {
        private readonly DataContext _context;

        public ChartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Chart chart)
        {
            var newChart = await _context.Charts.AddAsync(chart);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Chart> GetById(int id)
        {
            // Create a chartId object from the provided integer ID
            var chartId = ChartId.Create(id);

            // Query the database for the user with the specified ID
            var chart = await _context.Charts
                .FirstOrDefaultAsync(i => i.Id == chartId);

            return chart;
        }

        public async Task<int> DeleteById(int id)
        {
            // Create a chartId object from the provided integer ID
            var chartId = ChartId.Create(id);

            // Query the database for the user with the specified ID
            var chart = await _context.Charts
                .FirstOrDefaultAsync(i => i.Id == chartId);

            // Check if the interface was found
            if (chart == null)
            {
                throw new BusinessException(ErrorCode.NULL_ERROR, "Chart not found");
            }

            // Update the isDelete column and updateTime column
            chart.isDelete = 1;
            chart.updateTime = DateTime.Now;

            // Save the changes to the database
            var result = await _context.SaveChangesAsync();

            // Return the result of the deleted chart
            return result;
        }

        public async Task<int> Update(Chart chart)
        {
            // Attach the entity to the context and mark it as modified
            _context.Charts.Update(chart);
            // Save the changes to the database
            return await _context.SaveChangesAsync();
        }
    }
}
