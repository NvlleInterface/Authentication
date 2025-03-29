using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTracing;
using Microsoft.AspNetCore.Hosting;
using Authentication.Presentation;
using Authentication.Application;
using Authentication.Infrastructure;
using Authentication.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

IConfiguration configuration = configurationBuilder.Build();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .WriteTo.Console()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

using var listener = new ActivityListenerConfiguration()
    .Instrument.AspNetCoreRequests()
    .TraceToSharedLogger();

Log.Information("*** STARTUP ***");

var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(configuration!);
                if (configuration == null)
                {
                    throw new InvalidOperationException("Configuration not initialized");
                }

                //Add Entity cor
                // Create DefaultAdminsistrator User
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                services.AddInfrastructureCore(configuration, serviceProvider);
                services.AddApplicationCore(configuration);

            })
            .ConfigureLogging(logger =>
            {
                logger.ClearProviders();
                logger.AddConsole();
                logger.AddSerilog(dispose: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog()
            .Build();
host.Run();

