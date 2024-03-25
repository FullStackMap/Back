

using AutoMapper;
using Map.Domain.Entities;
using Map.Domain.Models.Testimonial;

namespace Map.API.AutoMapperProfies;

internal class TestimonialProfiles : Profile
{
    public TestimonialProfiles()
    {
        CreateMap<AddTestimonialDto, Testimonial>();
        CreateMap<Testimonial, TestimonialDto>();
    }
}
