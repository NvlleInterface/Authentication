using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TextswapAuthApi.Presentation.Configurations;

public static class AuthenticationRole
{
    public static async Task AddApplicationAdministrator(this IServiceCollection serviceDescriptors, IServiceProvider serviceProvider)
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
}
