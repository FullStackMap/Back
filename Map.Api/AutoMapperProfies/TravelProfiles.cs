using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.AddTravel;

namespace Map.API.AutoMapperProfies;

public class TravelProfiles : Profile
{
    public TravelProfiles()
    {
        CreateMap<AddTravelDto, Travel>()
            .ForMember(dest => dest.TravelRoad, opt => opt.MapFrom(src => new TravelRoad { RoadCoordinates = src.TravelRoad }));
    }
}
