using Microsoft.AspNetCore.Http;

namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IExcelService
    {
        Task<string> ConvertExcelToCsvAsync(IFormFile excelFile);
    }
}
