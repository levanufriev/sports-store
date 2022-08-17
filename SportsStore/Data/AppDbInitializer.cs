using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Data.Static;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SportsStore.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Suppliers.Any())
                {
                    string fileName = "Suppliers.json";
                    string jsonString = File.ReadAllText(fileName);
                    var suppliers = JsonSerializer.Deserialize<List<Supplier>>(jsonString);
                    context.Suppliers.AddRange(suppliers);
                    context.SaveChanges();
                }

                if (!context.Sports.Any())
                {
                    string fileName = "Sports.json";
                    string jsonString = File.ReadAllText(fileName);
                    var sports = JsonSerializer.Deserialize<List<KindOfSport>>(jsonString);
                    context.Sports.AddRange(sports);
                    context.SaveChanges();
                }

                if (!context.Categories.Any())
                {
                    string fileName = "Categories.json";
                    string jsonString = File.ReadAllText(fileName);
                    var categories = JsonSerializer.Deserialize<List<Category>>(jsonString);
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    string fileName = "Products.json";
                    string jsonString = File.ReadAllText(fileName);
                    var products = JsonSerializer.Deserialize<List<Product>>(jsonString);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "levanufriev46@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Lev Anufriev",
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "qwerty1234");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                
            }
        }
    }
}
