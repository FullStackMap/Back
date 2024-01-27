using AutoMapper;
using Map.API.Models.TripDto;
using Map.Domain.Entities;

namespace Map.API.AutoMapperProfies;

public class TripProfiles : Profile
{
    public TripProfiles()
    {
        CreateMap<AddTripDto, Trip>();
        CreateMap<Trip, TripDto>();
    }
}
