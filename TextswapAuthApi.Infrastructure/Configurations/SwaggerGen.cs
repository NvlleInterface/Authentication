
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TextswapAuthApi.Infrastructure.Configurations;

public static class SwaggerGen
{
    public static IServiceCollection AddSwaggerGenServices(this IServiceCollection services)
    {
        var service = services.AddSwaggerGen(option =>
       {
           option.SwaggerDoc("v1", new OpenApiInfo { Title = "TextswapAuthApiServer API", Version = "v1" });
           option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
           {
               In = ParameterLocation.Header,
               Description = "Please enter a valid token",
               Name = "Authorization",
               Type = SecuritySchemeType.Http,
               BearerFormat = "JWT",
               Scheme = "Bearer"
           });
           option.AddSecurityRequirement(new OpenApiSecurityRequirement
       {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
       });
       });

        return service;
    }
}
