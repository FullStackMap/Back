using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";

    public static string[] GetAllRoles() => new string[] { Admin, User };
}