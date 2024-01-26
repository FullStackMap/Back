using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.EFCore;

public class DBInitializer
{
    public static async Task<bool> Initialize(MapContext context, UserManager<MapUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        context.Database.EnsureCreated();


        if (context.Roles.Any() || context.Users.Any()) return false;


        //Adding roles
        var roles = Roles.GetAllRoles();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var resultAddRole = await roleManager.CreateAsync(new IdentityRole(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }

        //Adding Admin
        MapUser admin = new()
        {
            UserName = "Dercraker",
            Email = "antoine.capitain+MapPfe@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0606060606",
            PhoneNumberConfirmed = true,
        };

        string pwd = "NMdRx$HqyT8jX6";

        IdentityResult? resultAddUser = await userManager.CreateAsync(admin, pwd);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + resultAddUser.Errors);

        var resultAddRoleToUser = await userManager.AddToRoleAsync(admin, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);
        
        resultAddRoleToUser = await userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);


        await context.SaveChangesAsync();

        return true;
    }
}
