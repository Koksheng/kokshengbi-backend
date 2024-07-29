using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kokshengbi.Contracts.Chart
{
    public class GenChartByAiRequestWrapper
    {
        [FromForm]
        public IFormFile file { get; set; }

        //[FromForm]
        //public GenChartByAiRequest request { get; set; }

        /**
         * 图表名称
         */
        public string chartName { get; set; }

        /**
         * 分析目标
         */
        public string goal { get; set; }

        /**
         * 图表类型
         */
        public string chartType { get; set; }
    }
}
