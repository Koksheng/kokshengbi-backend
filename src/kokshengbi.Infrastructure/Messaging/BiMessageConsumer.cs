using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Domain.ChartAggregate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace kokshengbi.Infrastructure.Messaging
{
    public class BiMessageConsumer : IBiMessageConsumer
    {
        private readonly IChartRepository _chartRepository;
        private readonly IOpenAiService _openAiService;
        private readonly IModel _channel;

        public BiMessageConsumer(IChartRepository chartRepository, IOpenAiService openAiService)
        {
            _chartRepository = chartRepository;
            _openAiService = openAiService;

            //var factory = new ConnectionFactory() { HostName = "localhost" }; // Adjust hostname as needed
            //var connection = factory.CreateConnection();
            //_channel = connection.CreateModel();
        }

        public async Task ConsumeMessage(string message, ulong deliveryTag)
        {
            if (int.TryParse(message, out int chartId))
            {
                await ProcessMessage(chartId, deliveryTag);
            }
            else
            {
                // Handle invalid message format
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "Invalid message format");
            }
        }

        private async Task ProcessMessage(int chartId, ulong deliveryTag)
        {
            var chart = await _chartRepository.GetById(chartId);
            if (chart == null)
            {
                // Acknowledge message failure
                // _channel.BasicNack(deliveryTag, false, false);
                throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found");
            }

            // Update chart status to "running"
            chart.status = "running";
            await _chartRepository.Update(chart);

            try
            {
                var userInput = buildUserInput(chart);
                // Call AI service
                var result = await _openAiService.GenerateTextAsync(userInput);
                var parsedResponse = OpenAiResponseParser.ParseOpenAiResponse(result);

                // Update chart with the generated response
                chart.genChart = parsedResponse.echart.ToString();
                chart.genResult = parsedResponse.conclusion;
                chart.status = "succeed";
                await _chartRepository.Update(chart);

                // Acknowledge message
                // _channel.BasicAck(deliveryTag, false);
            }
            catch (Exception ex)
            {
                // Handle processing errors
                // _channel.BasicNack(deliveryTag, false, false);
                throw new Exception("Error processing message: " + ex.Message, ex);
            }
        }

        //public void StartConsuming()
        //{
        //    var consumer = new EventingBasicConsumer(_channel);
        //    consumer.Received += async (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        var message = Encoding.UTF8.GetString(body);
        //        var deliveryTag = ea.DeliveryTag;

        //        try
        //        {
        //            await ProcessMessage(int.Parse(message), deliveryTag);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception
        //            _channel.BasicNack(deliveryTag, false, false);
        //        }
        //    };

        //    _channel.BasicConsume(queue: BiMqConstant.BI_QUEUE_NAME,
        //                         autoAck: false,
        //                         consumer: consumer);
        //}

        //private async Task ProcessMessage(int chartId, ulong deliveryTag)
        //{
        //    var chart = await _chartRepository.GetById(chartId);
        //    if (chart == null)
        //    {
        //        _channel.BasicNack(deliveryTag, false, false);
        //        throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found");
        //    }

        //    // Update chart status to "running"
        //    chart.status = "running";
        //    await _chartRepository.Update(chart);

        //    try
        //    {
        //        var userInput = buildUserInput(chart);
        //        // Call AI service
        //        var result = await _openAiService.GenerateTextAsync(userInput);

        //        // Parse the response
        //        var parsedResponse = OpenAiResponseParser.ParseOpenAiResponse(result);

        //        // Update chart with the parsed result
        //        chart.genChart = parsedResponse.echart.ToString();
        //        chart.genResult = parsedResponse.conclusion;
        //        chart.status = "succeed";

        //        await _chartRepository.Update(chart);

        //        // Acknowledge the message
        //        _channel.BasicAck(deliveryTag, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        chart.status = "failed";
        //        chart.execMessage = ex.Message;
        //        await _chartRepository.Update(chart);
        //        _channel.BasicNack(deliveryTag, false, false);
        //    }
        //}

        private string buildUserInput(Chart chart)
        {
            string csvData = chart.chartData;
            string chartType = chart.chartType;
            string chartName = chart.chartName;
            string goal = chart.goal;

            StringBuilder userInput = new StringBuilder();
            // 压缩后的数据
            userInput.Append("Data in csv separated with comma:").Append("\n").Append(csvData);
            //userInput.Append("Chart Type：").Append("Bar Chart").Append(". \n");
            //userInput.Append("Requirement：").Append("You are a Data Analyst now. Please analyze the data with the chart type").Append(". \n");

            if (!string.IsNullOrEmpty(chartType))
            {
                userInput.Append("Chart Type：").Append(chartType).Append(". \n");
            }

            userInput.Append("Chart Name：").Append(chartName).Append(". \n")
            .Append("Requirement：").Append("You are a Data Analyst now. ").Append(goal).Append(". \n")

            .Append("Generate a response based on:").Append("\n")
            .Append("1. Echarts V5 in Json string for source of Echarts generation, set chartName as Echarts's title and chartType as Echarts's type (no comments). Ensure the JSON is correctly formatted for the specified chart type. For example:\n")
            .Append("   - For line charts: { title: { text: 'Chart Name' }, xAxis: { type: 'category', data: [...] }, yAxis: { type: 'value' }, series: [ { data: [...], type: 'line' } ] }\n")
            .Append("   - For pie charts: { title: { text: 'Chart Name', left: 'center' }, tooltip: { trigger: 'item' }, legend: { orient: 'vertical', left: 'left' }, series: [ { name: 'Access From', type: 'pie', radius: '50%', data: [...], emphasis: { itemStyle: { shadowBlur: 10, shadowOffsetX: 0, shadowColor: 'rgba(0, 0, 0, 0.5)' } } } ] }\n")
            .Append("   - For radar charts: { title: { text: 'Chart Name' }, legend: { data: ['Allocated Budget', 'Actual Spending'] }, radar: { indicator: [ { name: 'Sales', max: 6500 }, { name: 'Administration', max: 16000 }, { name: 'Information Technology', max: 30000 }, { name: 'Customer Support', max: 38000 }, { name: 'Development', max: 52000 }, { name: 'Marketing', max: 25000 } ] }, series: [ { name: 'Budget vs spending', type: 'radar', data: [ { value: [...], name: 'Allocated Budget' }, { value: [...], name: 'Actual Spending' } ] } ] }\n")
            .Append("2. Detailed analysis conclusions (no comments).").Append("\n")

            //Expected Result (must use this one, if not the response key will name as echrtsCode and analysis)
            .Append("Here is an example of expected response format. Please follow this format strictly.").Append("\n\n")
            .Append("Echart:").Append("\n")
            .Append("{ title: { text: 'Chart Name' }, xAxis: { type: 'category', data: ['1', '2', '3'] }, yAxis: { type: 'value' }, series: [ { data: [10, 20, 30], type: 'line' } ]};").Append("\n")
            .Append("Conclusion:").Append("\n")
            .Append("Based on the data analysis, the number of users shows a consistent increase over the three days. The number of users doubled from day 1 to day 2 and increased by 10 users each day, indicating a steady growth trend.").Append("\n");

            return userInput.ToString();
        }
    }
}
