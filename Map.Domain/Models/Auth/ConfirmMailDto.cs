using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Models.Auth;

public class ConfirmMailDto
{
    public ConfirmMailDto(string token, string email)
    {
        Token = token;
        Email = email;
    }

    public string Token { get; }
    public string Email { get; }
}
