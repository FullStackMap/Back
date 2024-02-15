using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Models.AuthDto;
public class TokenDto
{
    public TokenDto(string token, DateTime expirationDate)
    {
        Token = token;
        ExpirationDate = expirationDate;
    }

    public TokenDto(string token)
    {
        Token = token;
        ExpirationDate = null;
    }

    public string Token { get; set; }

    public DateTime? ExpirationDate { get; set; }
}