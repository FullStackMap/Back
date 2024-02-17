using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Models.AuthDto;

public class RegisterDto
{
    public RegisterDto(string username, string email, string password, string confirmPassword)
    {
        Username = username;
        Email = email;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
