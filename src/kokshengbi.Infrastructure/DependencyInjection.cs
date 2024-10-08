﻿using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Infrastructure.Persistence.Interceptors;
using kokshengbi.Infrastructure.Persistence.Repositories;
using kokshengbi.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using kokshengbi.Application.Common.Interfaces.Authentication;
using kokshengbi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Infrastructure.Services;
using StackExchange.Redis;
using kokshengbi.Infrastructure.Messaging;
using kokshengbi.Application.Common.Interfaces.Messaging;

namespace kokshengbi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuth(configuration)
                .AddPersistance(configuration)
                .AddService(configuration)
                .AddMessaging(configuration);

            return services;
        }

        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IExcelService, ExcelService>();

            // Configure HttpClient for YuCongMingClient
            services.AddHttpClient<IYuCongMingClient, YuCongMingClient>()
                    .ConfigureHttpClient(client =>
                    {
                        var accessKey = configuration["YuCongMing:AccessKey"];
                        var secretKey = configuration["YuCongMing:SecretKey"];
                        client.DefaultRequestHeaders.Add("AccessKey", accessKey);
                        client.DefaultRequestHeaders.Add("SecretKey", secretKey);
                    });

            // Register the OpenAiService
            var openAiApiKey = configuration["OpenAI:ApiKey"];
            services.AddSingleton<IOpenAiService, OpenAiService>(provider => new OpenAiService(openAiApiKey));

            // Configure Redis // Comment this on office pc, cause dont have Redis 
            //services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("127.0.0.1:6379"));
            //services.AddScoped<IRedisRateLimiterService, RedisRateLimiterService>();

            // Register ThreadPoolService
            services.AddSingleton<IThreadPoolService, ThreadPoolService>();

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Queue Messaging
            services.AddScoped<IBiMessageProducer, BiMessageProducer>();
            services.AddSingleton<IBiMessageConsumer, BiMessageConsumer>();
            // Register the hosted service
            services.AddHostedService<BiMessageConsumerHostedService>();

            return services;
        }

        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Register repositories
            services.AddScoped<PublishDomainEventsInterceptor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));  // Replace by below
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });

            return services;
        }
    }
}
