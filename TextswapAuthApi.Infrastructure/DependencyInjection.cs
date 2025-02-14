using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextswapAuthApi.Infrastructure.Configurations;
using TextswapAuthApi.Infrastructure.Context;

namespace TextswapAuthApi.Infrastructure;

public static class DependencyInjection
{
    public static IdentityBuilder AddIdentityCoreModule(this IServiceCollection serviceDescriptors)
    {
        var service = serviceDescriptors.AddIdentity<IdentityUser, IdentityRole>(o =>
        {
            o.User.RequireUniqueEmail = true;
            o.Password.RequireDigit = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireLowercase = false;
            o.Password.RequiredLength = 0;
        }).AddEntityFrameworkStores<AuthenticationContext>()
          .AddDefaultTokenProviders();

        return service;
    }

    public static IServiceCollection AddDbContextModule(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        var dbType = configuration["DatabaseType"];
        var connectionString = configuration.GetConnectionString(dbType);

        if (dbType == "PostgreSQL")
        {
            serviceDescriptors.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Cache"));
            return serviceDescriptors.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(connectionString));
        }
        else
        {
            return serviceDescriptors.AddDbContext<AuthenticationContext>(x => x.UseSqlServer(connectionString));
        }
    }

    public static IServiceCollection AddApplicationSwaggerGen(this IServiceCollection serviceSwagger)
    {
        return serviceSwagger.AddSwaggerGenServices();
    }

    public static IServiceCollection AddAuthenticationContextModule(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        return serviceDescriptors.AddDbContextModule(configuration);
    }

    public static IdentityBuilder AddAuthenticationIdentiteBuilderModule(this IServiceCollection serviceDescriptors)
    {
        var service = serviceDescriptors.AddIdentityCoreModule();

        return service;
    }
    public static async Task AddUserRoles(this IServiceCollection serviceUser, IServiceProvider serviceProvider)
    {
        await serviceUser.AddApplicationAdministrator(serviceProvider);
    }

    public static IServiceCollection AddConfigurationModule(this IServiceCollection serviceConfiguration, IConfiguration configuration)
    {
        (_, var authConfiguration) = serviceConfiguration.AddAuthConfiguration(configuration);
        serviceConfiguration.AddEmailConfiguration(configuration);

        serviceConfiguration.AddAuthenticationServices(authConfiguration);

        serviceConfiguration.AddPolicyModule();

        return serviceConfiguration;
    }
}
