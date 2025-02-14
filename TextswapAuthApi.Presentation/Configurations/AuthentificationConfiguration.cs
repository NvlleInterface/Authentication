using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TextswapAuthApi.Presentation.Configurations
{
    public static class AddTextswapAuthApiConfiguration
    {
        public static (IServiceCollection, TextswapAuthApiConfiguration) AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var TextswapAuthApiConfiguration = new TextswapAuthApiConfiguration();
            configuration.Bind("TextswapAuthApi", TextswapAuthApiConfiguration);
            return (services.AddSingleton(TextswapAuthApiConfiguration), TextswapAuthApiConfiguration);
        }

        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = new EmailConfiguration();
            configuration.Bind("EmailConfiguration", emailConfiguration);
            return services.AddSingleton(emailConfiguration);
        }

        public static AuthenticationBuilder AddAuthenticationServices(this IServiceCollection services, TextswapAuthApiConfiguration configuration)
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
                                context.Response.Headers.Add
                                ("Authentication-Token-Expired", "true");
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
    }
}
