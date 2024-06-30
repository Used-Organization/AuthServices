using AuthServices.Domain.Models;
using AuthServices.Infrastructure.Data;
using AuthServices.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServices.API
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
        }

        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
        public static void AddTestingAdminUser(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                SeedAdminData(userManager, roleManager).GetAwaiter().GetResult();
            }
        }
        async static Task SeedAdminData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            var adminUser = new ApplicationUser
            {
                FirstName="Testing",
                LastName="Admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                CreatedByIp = "127.0.0.1"
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "AdminPassword123!");
                await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.SystemAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.SystemAdmin));
            }

            var SystemAdmin = new ApplicationUser
            {
                FirstName = "Testing",
                LastName = "SystemAdmin",
                UserName = "sysAdmin@admin.com",
                Email = "sysAdmin@admin.com",
                EmailConfirmed = true,
                CreatedByIp="127.0.0.1"
            };

            if (await userManager.FindByEmailAsync(SystemAdmin.Email) == null)
            {
                await userManager.CreateAsync(SystemAdmin, "AdminPassword123!");
                await userManager.AddToRoleAsync(SystemAdmin, UserRoles.SystemAdmin);
            }
        }
    }

}
