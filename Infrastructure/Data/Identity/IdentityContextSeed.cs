using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            
            if(!userManager.Users.Any())
            {
                
                var user = new User
                {
                    UserName = "Testing",
                    DisplayName = "Tom",
                    Email = "Tom@website.com",
                    Address = new Address
                    {
                        FirstName = "Tom",
                        LastMidofiedBy = "Tomily",
                        Street = "Street",
                        City = "City",
                        PostCode = "55555"
                    }
                };
                var answer = await userManager.CreateAsync(user, "Test$123456");  
                            
                if(!answer.Succeeded){
                    foreach (var item in answer.Errors)
                    {
                        Console.WriteLine($"-> {item.Description}");
                    }
                }
            }
        }
    }
}