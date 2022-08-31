using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            
            if(!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        UserName = "Testing",
                        DisplayName = "Tom",
                        Email = "Tom@website.com",
                        Address = new Address
                        {
                            FirstName = "Tom",
                            LastMidofiedBy = null,
                            Street = "Street",
                            City = "City",
                            PostCode = "55555"
                        }
                    },
                    new User
                    {
                        DisplayName = "Admin",
                        Email = "admin@test.com",
                        UserName = "Admin"
                    }
                };

                var roles = new List<Role>
                {
                    new Role {Name = "Admin"},
                    new Role {Name = "Member"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Test$123456");
                    await userManager.AddToRoleAsync(user, "Member");
                    if (user.Email == "admin@test.com") await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}