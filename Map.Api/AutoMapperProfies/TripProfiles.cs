using AutoMapper;
using Map.API.Models.TripDto;
using Map.Domain.Entities;
using Map.Domain.Models.TripDto;

namespace Map.API.AutoMapperProfies;

public class TripProfiles : Profile
{
    public TripProfiles()
    {
        CreateMap<AddTripDto, Trip>();
        CreateMap<UpdateTripDto, Trip>();
        CreateMap<Trip, TripDto>();
    }
}
