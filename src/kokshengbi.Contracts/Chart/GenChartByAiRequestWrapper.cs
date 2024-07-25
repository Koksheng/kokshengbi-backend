using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kokshengbi.Contracts.Chart
{
    public class GenChartByAiRequestWrapper
    {
        [FromForm]
        public IFormFile file { get; set; }

        [FromForm]
        public GenChartByAiRequest request { get; set; }
    }
}
