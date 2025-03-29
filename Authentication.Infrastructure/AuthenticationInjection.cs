
using Authentication.Domain.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.Infrastructure;

public static class AuthenticationInjection
{
    public static async Task AddRoleCore(this IServiceCollection serviceDescriptors, IServiceProvider serviceProvider)
    {
        try
        {
            // retrive instances of the RoleManager and UserManager 
            //from the Dependency Container
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult result;
            var roles = new Dictionary<string, string> { { "admin@email.com", "Admin" }, { "user@email.com", "User" } };

            // add a new role for the application
            foreach (var (key, value) in roles)
            {
                var isRoleExist = await roleManager
                .RoleExistsAsync(value);
                if (!isRoleExist)
                {
                    // create Admin/User Role and add it in Database
                    result = await roleManager
                        .CreateAsync(new IdentityRole(value));
                }

                // code to create a default user/admin and add it role
                var user = await userManager
                .FindByEmailAsync(key);
                if (user == null)
                {
                    var defaultUser = new IdentityUser()
                    {
                        UserName = key,
                        Email = key
                    };
                    var regUser = await userManager
                        .CreateAsync(defaultUser, "@P0wrd2023");
                    await userManager
                        .AddToRoleAsync(defaultUser, value);
                }
            }
        }
        catch (Exception ex)
        {
            var str = ex.Message;
        }

    }

    public static (IServiceCollection, AuthenticationConfiguration) AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfiguration = new AuthenticationConfiguration();
        configuration.Bind("AuthenticationConfiguration", authConfiguration);
        return (services.AddSingleton(authConfiguration), authConfiguration);
    }

    public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfiguration = new EmailConfiguration();
        configuration.Bind("EmailConfiguration", emailConfiguration);
        return services.AddSingleton(emailConfiguration);
    }

    public static AuthenticationBuilder AddAuthenticationCore(this IServiceCollection services, AuthenticationConfiguration configuration)
    {
        // Adding Authentication
        var service = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = configuration.Audience,
                    ValidIssuer = configuration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.AccessTokenSecret))
                };
                options.Events = new JwtBearerEvents()
                {
                    // If the Token is expired the respond
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers?.Append("Authentication-Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }).AddCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied =
                options.Events.OnRedirectToLogin = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.FromResult<object>(null!);
                };
            });

        return service;
    }

    public static IServiceCollection AddPolicyCore(this IServiceCollection servicePolicy)
    {
        //servicePolicy.AddCors(options =>
        //{
        //    options.AddPolicy("corspolicy", (policy) =>
        //    {
        //        policy.WithOrigins("http://localhost:4200")
        //              .AllowAnyOrigin()
        //              .AllowAnyMethod()
        //              .AllowAnyHeader();
        //    });
        //});
        servicePolicy.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", (policy) =>
            {
                policy.RequireRole("Admin");
            });

            options.AddPolicy("AdminManagerPolicy", (policy) =>
            {
                policy.RequireRole("Admin", "Manager");
            });

            options.AddPolicy("AdminManagerStandardPolicy", (policy) =>
            {
                policy.RequireRole("Admin", "Manager", "Standard");
            });
        });

        return servicePolicy;
    }

}
