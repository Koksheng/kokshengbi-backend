To implement rate limiting in your .NET Core application, you can use Redis and the `StackExchange.Redis` library. You can implement a rate limiter that checks the number of requests a user makes within a specified time window and enforce the limit accordingly.

Here's how you can implement rate limiting in your `GenChartByAiCommandHandler.cs`:

1. Install Redis and StackExchange.Redis:

    - Ensure you have Redis installed and running.

    - Install the StackExchange.Redis NuGet package in your .NET Core project:

    ```
    dotnet add package StackExchange.Redis
    ```

2. Create a Redis Rate Limiter Service:
Create a service that will handle rate limiting using Redis.

    ```
    using StackExchange.Redis;
    using System;

    public class RedisRateLimiterService
    {
        private readonly IDatabase _database;

        public RedisRateLimiterService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period)
        {
            var currentCount = await _database.StringIncrementAsync(key);
            if (currentCount == 1)
            {
                await _database.KeyExpireAsync(key, period);
            }

            return currentCount <= limit;
        }
    }
    ```

3. Register the Redis and Rate Limiter Service:
Register the Redis connection and the rate limiter service in your `Startup.cs` or `Program.cs`.

    ```
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Other service registrations...

            var redis = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddSingleton<RedisRateLimiterService>();
        }

        // Other configurations...
    }
    ```
 
4. Update GenChartByAiCommandHandler to Use Rate Limiting:
Update your GenChartByAiCommandHandler to use the rate limiter service.

```
public class GenChartByAiCommandHandler : IRequestHandler<GenChartByAiCommand, BIResult>
{
    private readonly RedisRateLimiterService _rateLimiterService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IExcelService _excelService;
    private readonly IOpenAiService _openAiService;
    private readonly IMapper _mapper;
    private readonly IChartRepository _chartRepository;

    public GenChartByAiCommandHandler(
        RedisRateLimiterService rateLimiterService,
        ICurrentUserService currentUserService,
        IExcelService excelService,
        IOpenAiService openAiService,
        IMapper mapper,
        IChartRepository chartRepository)
    {
        _rateLimiterService = rateLimiterService;
        _currentUserService = currentUserService;
        _excelService = excelService;
        _openAiService = openAiService;
        _mapper = mapper;
        _chartRepository = chartRepository;
    }

    public async Task<BIResult> Handle(GenChartByAiCommand command, CancellationToken cancellationToken)
    {
        string chartName = command.chartName;
        string goal = command.goal;
        string chartType = command.chartType;
        string userState = command.userState;
        var file = command.file;

        var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

        // Rate limiting
        string rateLimitKey = $"genChartByAi:{safetyUser.Id}";
        bool isAllowed = await _rateLimiterService.IsAllowedAsync(rateLimitKey, 2, TimeSpan.FromSeconds(1));
        if (!isAllowed)
        {
            throw new BusinessException(ErrorCode.TO_MANY_REQUEST, "该服务被调用次数过多");
        }

        // Validate file
        if (file == null)
        {
            throw new BusinessException(ErrorCode.PARAMS_ERROR, "File is empty");
        }

        const long ONE_MB = 1024 * 1024;
        if (file.Length > ONE_MB)
        {
            throw new BusinessException(ErrorCode.PARAMS_ERROR, "文件超过 1M, Files larger than 1M");
        }

        string originalFilename = file.FileName;
        string suffix = Path.GetExtension(originalFilename).TrimStart('.').ToLower();
        var validFileSuffixList = new List<string> { "xlsx" };

        if (!validFileSuffixList.Contains(suffix))
        {
            throw new BusinessException(ErrorCode.PARAMS_ERROR, "文件后缀非法, Illegal file extension");
        }

        // Process file and generate response
        var csvData = await _excelService.ConvertExcelToCsvAsync(file);

        StringBuilder userInput = new StringBuilder();
        userInput.Append("Data in csv separated with comma:").Append("\n").Append(csvData);

        if (!string.IsNullOrEmpty(chartType))
        {
            userInput.Append("Chart Type：").Append(chartType).Append(". \n");
        }

        userInput.Append("Chart Name：").Append(chartName).Append(". \n")
                .Append("Requirement：").Append("You are a Data Analyst now. ").Append(goal).Append(". \n")
                .Append("Generate a response based on:").Append("\n")
                .Append("1. Echarts V5 in Json string for source of Echarts generation, set chartName as Echarts's title and chartType as Echarts's type (no comments). Ensure the JSON is correctly formatted for the specified chart type.").Append("\n")
                .Append("2. Detailed analysis conclusions (no comments).").Append("\n")
                .Append("Here is an example of expected response format. Please follow this format strictly.").Append("\n\n")
                .Append("Echart:").Append("\n")
                .Append("{ title: { text: 'Chart Name' }, xAxis: { type: 'category', data: ['1', '2', '3'] }, yAxis: { type: 'value' }, series: [ { data: [10, 20, 30], type: 'line' } ]};").Append("\n")
                .Append("Conclusion:").Append("\n")
                .Append("Based on the data analysis, the number of users shows a consistent increase over the three days. The number of users doubled from day 1 to day 2 and increased by 10 users each day, indicating a steady growth trend.").Append("\n");

        var openAiResponse = await _openAiService.GenerateTextAsync(userInput.ToString());
        var parsedResponse = OpenAiResponseParser.ParseOpenAiResponse(openAiResponse);

        Chart chart = _mapper.Map<Chart>(parsedResponse);
        chart.goal = goal;
        chart.chartName = chartName;
        chart.chartData = csvData;
        chart.chartType = chartType;
        chart.status = "wait";
        chart.userId = safetyUser.Id;
        chart.createTime = DateTime.Now;
        chart.updateTime = DateTime.Now;

        var result = await _chartRepository.Add(chart);

        if (result == 1)
        {
            return _mapper.Map<BIResult>(chart);
        }
        else
        {
            throw new BusinessException(ErrorCode.OPERATION_ERROR);
        }
    }
}
```

This implementation leverages Redis to manage rate limiting, ensuring that each user can only make a specified number of requests within a given time period. If a user exceeds the limit, a `BusinessException` is thrown, and the request is not processed.