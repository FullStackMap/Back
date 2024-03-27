using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.AddTravel;
using Map.Domain.Models.Travels;

namespace Map.API.AutoMapperProfies;

internal class TravelProfiles : Profile
{
    public TravelProfiles()
    {
        CreateMap<AddTravelDto, Travel>()
            .ForMember(dest => dest.TravelRoad, opt => opt.MapFrom(src => new TravelRoad { RoadCoordinates = src.TravelRoad }));

        CreateMap<Travel, TravelDto>();
        CreateMap<Travel, TravelDtoList>();
        CreateMap<TravelRoad, TravelRoadDto>();
    }
}
