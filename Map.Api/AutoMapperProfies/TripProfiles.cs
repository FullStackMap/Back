using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.Trip;

namespace Map.API.AutoMapperProfies;

internal class TripProfiles : Profile
{
    public TripProfiles()
    {
        CreateMap<AddTripDto, Trip>();
        CreateMap<UpdateTripDto, Trip>();
        CreateMap<Trip, TripDto>();
    }
}
