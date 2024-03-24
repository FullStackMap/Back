using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.Auth;
using Map.Domain.Models.EmailDto;
using Map.Domain.Models.User;

namespace Map.API.AutoMapperProfies;

internal class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<RegisterDto, MapUser>();
        CreateMap<MapUser, MapUserDto>();
        CreateMap<SupportContactMailDto, MailDto>();
    }
}