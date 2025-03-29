using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Common.Services.TokenGenerators;
using Authentication.Application.Common.Services;
using Authentication.Application.Common.Services.PasswordHashers;
using Authentication.Application.Common.Services.RefreshTokenRepositories;
using Authentication.Domain.Models;
using Authentication.Application.Common.Services.MailMessage;

namespace Authentication.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        serviceDescriptors.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        serviceDescriptors.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                                  .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        serviceDescriptors.AddSingleton<TokenGenerator>()
                            .AddSingleton<AccessTokenGenerator>()
                            .AddSingleton<RefreshTokenGenerator>()
                            .AddSingleton<RefreshTokenValidator>()
                            .AddScoped<Authentications>()
                            .AddSingleton<IPasswordHasher, BcryptPasswordHasher>()
                            .AddScoped<IRefreshTokenRepository<RefreshToken>, RefreshTokenRepository>()
                            .AddSingleton<IEmailService, EmailService>()
                            .AddHttpContextAccessor();

        return serviceDescriptors;

    }
}