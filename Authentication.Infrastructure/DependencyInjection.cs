using Authentication.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure;

public static class DependencyInjection
{
    public static IdentityBuilder AddInfrastructureCore(this IServiceCollection services, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var service = services.AddIdentity<IdentityUser, IdentityRole>(o =>
        {
            o.User.RequireUniqueEmail = true;
            o.Password.RequireDigit = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireLowercase = false;
            o.Password.RequiredLength = 0;
        }).AddEntityFrameworkStores<AuthenticationContext>()
          .AddDefaultTokenProviders();

        var dbType = configuration["DatabaseType"];
        var connectionString = configuration.GetConnectionString(dbType!);

        if (dbType == "PostgreSQL")
        {
            services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Cache"));
            services.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(connectionString));
        }
        services.AddRoleCore(serviceProvider);

        (_, var authConfiguration) = services.AddAuthConfiguration(configuration);
        services.AddEmailConfiguration(configuration);

        services.AddAuthenticationCore(authConfiguration);
        services.AddPolicyCore();

        using (var scope = serviceProvider.CreateScope())
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

        return service;
    }
}
