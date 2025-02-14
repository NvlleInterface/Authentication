using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTracing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TextswapAuthApi.Presentation;
using TextswapAuthApi.Infrastructure.Context;
using TextswapAuthApi.Infrastructure;
using TextswapAuthApi.Application;

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
    ///.Instrument.AspNetCoreRequests()
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
                services.AddAuthenticationContextModule(configuration);
                services.AddConfigurationModule(configuration);

                services.AddAuthenticationIdentiteBuilderModule();

                services.AddEndpointsApiExplorer();
                services.AddApplicationSwaggerGen();

                services.AddDependencyModule(configuration);

                // Create DefaultAdminsistrator User
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                Task task = services.AddUserRoles(serviceProvider);

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
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
                    Console.WriteLine("Applying migrations...");
                    context.Database.Migrate();

                    var pendingMigrations = context.Database.GetPendingMigrations();

                    if (pendingMigrations.Any())
                    {
                        Console.WriteLine($"Current database migration version: {pendingMigrations.Last()}");
                        Console.WriteLine("Migrations applied successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }

host.Run();

