using System.Reflection;
using FluentValidation.AspNetCore;
using TextswapAuthApi.Application.Common.Services.TokenGenerators;
using TextswapAuthApi.Application.Common.Services.PasswordHashers;
using TextswapAuthApi.Application.Common.Services.MailMessage;
using TextswapAuthApi.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using TextswapAuthApi.Application.Common.Services.RefreshTokenRepositories;
using TextswapAuthApi.Domaine.Models.Authentication.Entities;
using Microsoft.Extensions.Configuration;

namespace TextswapAuthApi.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddDependencyModule(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        serviceDescriptors
                 .AddFluentValidation(options =>
                 {
                     // Validate child properties and root collection elements
                     options.ImplicitlyValidateChildProperties = true;
                     options.ImplicitlyValidateRootCollectionElements = true;
                     // Automatic registration of validators in assembly
                     options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                 });

        serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                                  .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        serviceDescriptors.AddSingleton<TokenGenerator>()
                            .AddSingleton<AccessTokenGenerator>()
                            .AddSingleton<RefreshTokenGenerator>()
                            .AddSingleton<RefreshTokenValidator>()
                            .AddScoped<Authentication>()
                            .AddSingleton<IPasswordHasher, BcryptPasswordHasher>()
                            .AddScoped<IRefreshTokenRepository<RefreshToken>, RefreshTokenRepository>()
                            .AddSingleton<IEmailService, EmailService>()
                            .AddHttpContextAccessor();

        return  serviceDescriptors;

    }
}
