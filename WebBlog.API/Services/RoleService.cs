using Microsoft.AspNetCore.Identity;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Models;

namespace WebBlog.API.Services
{
    public class RoleService
    {
        public static async Task CreateRoles(IServiceProvider services)
        {
            using var scope = services.CreateScope(); // Create a new scope to obtain scoped services from the DI container 
            var context = scope.ServiceProvider.GetRequiredService<BlogDatabase>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RoleService>>();//

            try
            {
                await context.Database.EnsureCreatedAsync();
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");

                var adminEmail = "admin@haukong.com";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new AppUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        NormalizedEmail = adminEmail.ToUpper(),
                        NormalizedUserName = adminEmail.ToUpper(),
                    };
                    var result = await userManager.CreateAsync(admin, "Admin1234");
                    if(result.Succeeded)
                    {
                        logger.LogInformation("Admin created");
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                    else
                    {
                        logger.LogError("Faile to create admin user: {Errors}", string.Join(", ", result.Errors.Select(x => x.Description)));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while seeding the database");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if(!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if(!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}");
                }
            }
        }
    }
}
