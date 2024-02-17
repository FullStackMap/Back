using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Map.EFCore;

public class DBInitializer
{
    private readonly MapContext _context;
    private readonly UserManager<MapUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DBInitializer"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="roleManager">The role manager.</param>
    public DBInitializer(MapContext context, UserManager<MapUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Initializes the.
    /// </summary>
    /// <returns>A Task.</returns>
    public async Task<bool> Initialize()
    {
        _context.Database.EnsureCreated();
        await _context.MapUser.ExecuteDeleteAsync();
        await _context.Trip.ExecuteDeleteAsync();

        if (_context.Roles.Any() && _context.Users.Any())
            return false;

        #region AddRoles

        string[] roles = Roles.GetAllRoles();

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var resultAddRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }

        await _context.SaveChangesAsync();

        #endregion

        #region AddAdmin

        MapUser admin = new()
        {
            UserName = "Dercraker",
            Email = "antoine.capitain+MapPfe@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0606060606",
            PhoneNumberConfirmed = true,
        };

        string pwd = "NMdRx$HqyT8jX6";

        IdentityResult? resultAddUser = await _userManager.CreateAsync(admin, pwd);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + resultAddUser.Errors);

        var resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);

        resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);

        await _context.SaveChangesAsync();

        #endregion

        #region AddUser
        //Adding Admin
        List<MapUser> mapUsers = new List<MapUser>
{
    new MapUser
    {
        UserName = "Dercraker2",
        Email = "antoine.capitain+MapUserPfe@gmail.com",
        EmailConfirmed = true,
        PhoneNumber = "(600) 890-5820",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "MikeStewart",
        Email = "go@an.fj",
        EmailConfirmed = true,
        PhoneNumber = "(859) 897-8305",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "RosaBell",
        Email = "tew@ron.mr",
        EmailConfirmed = true,
        PhoneNumber = "(901) 951-7529",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "PaulineGarza",
        Email = "sokinva@pugbe.np",
        EmailConfirmed = true,
        PhoneNumber = "(201) 658-4063",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "MikeFernandez",
        Email = "na@zal.ck",
        EmailConfirmed = true,
        PhoneNumber = "(414) 721-6737",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "HallieDawson",
        Email = "zocinbez@paegwe.bh",
        EmailConfirmed = true,
        PhoneNumber = "(767) 688-2269",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "GertrudeUnderwood",
        Email = "elbovdu@legdekesu.gi",
        EmailConfirmed = true,
        PhoneNumber = "(470) 818-1006",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "EllenJohnson",
        Email = "ji@jag.cw",
        EmailConfirmed = true,
        PhoneNumber = "(821) 960-5640",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "BobbyBates",
        Email = "avpug@fu.gh",
        EmailConfirmed = true,
        PhoneNumber = "(810) 266-3141",
        PhoneNumberConfirmed = true,
    },
    new MapUser
    {
        UserName = "AmandaFowler",
        Email = "puomi@lik.bf",
        EmailConfirmed = true,
        PhoneNumber = "(265) 905-6736",
        PhoneNumberConfirmed = true,
    }
};

        Random random = new Random();
        foreach (MapUser user in mapUsers)
        {
            user.EmailConfirmed = random.Next(2) == 0;
            user.PhoneNumberConfirmed = random.Next(2) == 0;

            resultAddUser = await _userManager.CreateAsync(user, pwd);
            if (!resultAddUser.Succeeded)
                throw new ApplicationException("Adding user '" + user.UserName + "' failed with error(s): " + resultAddUser.Errors);

            resultAddRoleToUser = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!resultAddRoleToUser.Succeeded)
                throw new ApplicationException("Adding user '" + user.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);
            await _context.SaveChangesAsync();

        }

        #endregion
        return true;
    }
}
