﻿using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Map.EFCore.Data;

public class DBInitializer
{
    private readonly MapContext _context;
    private readonly UserManager<MapUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly Random _random;

    private const string _password = "NMdRx$HqyT8jX6";

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
        _random = new Random();
    }

    /// <summary>
    /// Initializes the.
    /// </summary>
    /// <returns>A Task.</returns>
    public async Task<bool> Initialize(bool force)
    {

        _context.Database.EnsureCreated();

        if (!force && _context.Roles.Any() && _context.Users.Any())
            return false;

        await ClearDatabase();

        await AddRolesAsync();
        await AddAdminAsync();
        await AddSingleUserAsync();
        await AddUsersAsync();
        await AddTripsAsync();
        await GenerateTestimonialsAsync();

        return true;
    }

    private async Task ClearDatabase()
    {
        await _context.TravelRoad.ExecuteDeleteAsync();
        await _context.Travel.ExecuteDeleteAsync();
        await _context.Trip.ExecuteDeleteAsync();
        await _context.MapUser.ExecuteDeleteAsync();
        await _context.Users.ExecuteDeleteAsync();
        await _context.Roles.ExecuteDeleteAsync();
    }

    private async Task AddRolesAsync()
    {
        string[] roles = Roles.GetAllRoles();

        foreach (string role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                IdentityResult resultAddRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task AddAdminAsync()
    {
        MapUser userAdmin = new()
        {
            UserName = "Dercraker",
            Email = "dercraker@from-a2b.fr",
            EmailConfirmed = true,
            PhoneNumber = "0606060606",
            PhoneNumberConfirmed = true,
        };

        IdentityResult? resultAddUser = await _userManager.CreateAsync(userAdmin, _password);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + userAdmin.UserName + "' failed with error(s): " + resultAddUser.Errors);

        IdentityResult resultAddRoleToUser = await _userManager.AddToRoleAsync(userAdmin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + userAdmin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);

        resultAddRoleToUser = await _userManager.AddToRoleAsync(userAdmin, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + userAdmin.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);

        await _context.SaveChangesAsync();
    }

    private async Task AddSingleUserAsync()
    {
        MapUser simpleUser = new()
        {
            UserName = "Dercraker2",
            Email = "dercraker2@from-a2b.fr",
            EmailConfirmed = true,
            PhoneNumber = "(600) 890-5820",
            PhoneNumberConfirmed = true,
        };


        IdentityResult resultAddUser = await _userManager.CreateAsync(simpleUser, _password);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + simpleUser.UserName + "' failed with error(s): " + resultAddUser.Errors);

        IdentityResult resultAddRoleToUser = await _userManager.AddToRoleAsync(simpleUser, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + simpleUser.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);

        await _context.SaveChangesAsync();
    }

    private async Task AddUsersAsync()
    {
        List<MapUser> mapUsers = new List<MapUser>
        {
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
        };

        Random random = new Random();
        foreach (MapUser user in mapUsers)
        {
            user.EmailConfirmed = random.Next(2) == 0;
            user.PhoneNumberConfirmed = random.Next(2) == 0;

            IdentityResult resultAddUser = await _userManager.CreateAsync(user, _password);
            if (!resultAddUser.Succeeded)
                throw new ApplicationException("Adding user '" + user.UserName + "' failed with error(s): " + resultAddUser.Errors);

            IdentityResult resultAddRoleToUser = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!resultAddRoleToUser.Succeeded)
                throw new ApplicationException("Adding user '" + user.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);
            await _context.SaveChangesAsync();
        }
    }

    private async Task AddTripsAsync()
    {
        foreach (MapUser user in _context.MapUser.ToList())
        {
            int numTrips = _random.Next(2, 11);
            for (int i = 0; i < numTrips; i++)
            {
                Trip trip = new Trip
                {
                    User = user,
                    Name = $"Trip {i + 1} for {user.UserName}",
                    Description = $"Description for Trip {i + 1} for {user.UserName}",
                    StartDate = new DateOnly(_random.Next(2022, 2025), _random.Next(1, 13), _random.Next(1, 29)),
                    EndDate = new DateOnly(_random.Next(2022, 2025), _random.Next(1, 13), _random.Next(1, 29)),
                    BackgroundPicturePath = "https://via.placeholder.com/150"
                };

                await _context.Trip.AddAsync(trip);

                int numSteps = _random.Next(2, 6);
                List<Step> steps = new List<Step>();

                for (int j = 0; j < numSteps; j++)
                {
                    Step step = new Step
                    {
                        TripId = trip.TripId,
                        Order = j + 1,
                        Name = $"Step {j + 1} for Trip {i + 1} of user {user.UserName}",
                        Longitude = (decimal)(_random.NextDouble() * 360 - 180),
                        Latitude = (decimal)(_random.NextDouble() * 180 - 90),
                        Description = $"Description for Step {j + 1} for Trip {i + 1}",
                        StartDate = DateTime.Now.AddDays(j),
                        EndDate = DateTime.Now.AddDays(j + 1)
                    };

                    steps.Add(step);

                    trip.Steps = steps;
                }
                await _context.SaveChangesAsync();

                for (int k = 0; k < steps.Count - 1; k++)
                {
                    Travel travel = new Travel
                    {
                        TripId = trip.TripId,
                        OriginStep = steps[k],
                        DestinationStep = steps[k + 1],
                        TransportMode = "Mode of Transport",
                        Distance = (decimal)_random.NextDouble() * 1000,
                        Duration = (decimal)_random.NextDouble() * 24
                    };
                    TravelRoad travelRoad = new TravelRoad
                    {
                        TravelId = travel.TravelId,
                        Travel = travel,
                        RoadCoordinates = "Coordinates of Road"
                    };
                    travel.TravelRoad = travelRoad;
                    steps[k].TravelAfter = travel;

                    await _context.SaveChangesAsync();
                }
            }
        }
    }

    private async Task GenerateTestimonialsAsync()
    {
        foreach (MapUser user in _context.MapUser.ToList())
        {
            int numTestimonials = _random.Next(1, 4);
            for (int i = 0; i < numTestimonials; i++)
            {
                Testimonial testimonial = new Testimonial
                {
                    FeedBack = GenerateRandomFeedback(),
                    UserId = user.Id,
                    User = user,
                    Rate = _random.Next(1, 6),
                    TestimonialDate = new DateOnly(_random.Next(2022, 2025), _random.Next(1, 13), _random.Next(1, 29))
                };

                _context.Testimonial.Add(testimonial);

            }
        }

        await _context.SaveChangesAsync();
    }

    private string GenerateRandomFeedback()
    {
        string[] feedbacks = new string[]
        {
                "Excellent service!",
                "Très satisfait de mon voyage.",
                "Personnel très professionnel.",
                "Expérience incroyable.",
                "Je recommande vivement!"
        };

        return feedbacks[_random.Next(feedbacks.Length)];
    }


}

