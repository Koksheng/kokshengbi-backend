using kokshengbi.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Text;

namespace kokshengbi.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<string> ConvertExcelToCsvAsync(IFormFile excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new MemoryStream();
            await excelFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null) return string.Empty;

            var csv = new StringBuilder();
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            for (int row = 1; row <= rowCount; row++)
            {
                var rowValues = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Text;
                    rowValues.Add(cellValue);
                }
                csv.AppendLine(string.Join(",", rowValues));
            }

            return csv.ToString();
        }
    }
}
