using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextswapAuthApi.Presentation.Configurations
{
    public static class AuthorizationPolicy
    {
        public static IServiceCollection AddPolicyModule(this IServiceCollection servicePolicy)
        {
            servicePolicy.AddCors(options =>
            {
                options.AddPolicy("corspolicy", (policy) =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
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
}
