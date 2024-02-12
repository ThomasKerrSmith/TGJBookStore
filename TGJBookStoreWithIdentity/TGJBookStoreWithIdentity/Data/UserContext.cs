using Microsoft.AspNetCore.Identity;
using TGJBookStoreWithIdentity.Models;


//ALL COMMENTS BY JONATHAN SIVANANTHAN 9NOV


namespace TGJBookStoreWithIdentity.Data
{
    public static class UserContext
    {
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //This class will seed a few default values to the database in case this project is used with a new db
            //Again we will use userManager and roleManager class methods

            //seed a default admin account (for owner)
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@tgj.com",
                FirstName = "Larry",
                LastName = "Owner",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(a => a.Id != adminUser.Id)) 
            {
                var user = await userManager.FindByEmailAsync(adminUser.Email); 

                if(user == null) //if adminUser doesnt alrady exist, create one and give all the roles 
                {
                    await userManager.CreateAsync(adminUser, "Password@123");
                    await userManager.AddToRoleAsync(adminUser, Enums.Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(adminUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(adminUser, Enums.Roles.Staff.ToString());
                    await userManager.AddToRoleAsync(adminUser, Enums.Roles.User.ToString());
                }
            }
        }


        //this method will seed role data to the db from the roles enum cs file
        public static async Task SeedRolesAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
            {
            //Seed these Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Staff.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.User.ToString()));
        }

    }
}
