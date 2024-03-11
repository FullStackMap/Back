using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.Step;

namespace Map.API.AutoMapperProfies;

public class StepProfiles : Profile
{
    public StepProfiles()
    {
        CreateMap<AddStepDto, Step>();
        CreateMap<Step, StepDto>();
    }
}