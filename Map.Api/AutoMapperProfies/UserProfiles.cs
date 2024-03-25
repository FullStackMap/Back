using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.Auth;

namespace Map.API.AutoMapperProfies;

internal class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<RegisterDto, MapUser>();
        CreateMap<MapUser, MapUserDto>();
    }
}