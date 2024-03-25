namespace Map.Domain.Entities;

public class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";

    public static string[] GetAllRoles() => new string[] { Admin, User };
}