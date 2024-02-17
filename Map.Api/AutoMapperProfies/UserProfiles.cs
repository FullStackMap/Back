using AutoMapper;
using Map.API.Models.TripDto;
using Map.Domain.Entities;
using Map.Domain.Models.AuthDto;
using Map.Domain.Models.TripDto;

namespace Map.API.AutoMapperProfies;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<RegisterDto, MapUser>();
        CreateMap<MapUser, MapUserDto>();
    }
}