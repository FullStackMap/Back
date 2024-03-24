using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;
public interface ITestimonialRepository : IGenericRepository<Testimonial>
{
    /// <summary>
    /// Add testimonial to user
    /// </summary>
    /// <param name="user">User to add Testimonial</param>
    /// <param name="testimonial">Testimonial to add</param>
    Task AddTestimonial(MapUser user, Testimonial testimonial);
}
