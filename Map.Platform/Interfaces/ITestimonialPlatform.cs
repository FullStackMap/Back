using Map.Domain.Entities;

namespace Map.Platform.Interfaces;
public interface ITestimonialPlatform
{
    /// <summary>
    /// Add testimonial to the user
    /// </summary>
    /// <param name="user">User to add testimonial to</param>
    /// <param name="testimonial">Testimonial to add</param>
    Task AddTestimonialAsync(MapUser user, Testimonial testimonial);

    /// <summary>
    /// Delete testimonial
    /// </summary>
    /// <param name="testimonial">testimonial to delete</param>
    Task DeleteTestimonialAsync(Testimonial testimonial);

    /// <summary>
    /// Get all testimonials
    /// </summary>
    IQueryable<Testimonial> GetAllTestimonials();

    /// <summary>
    /// Get testimonial by id for the user
    /// </summary>
    /// <param name="testimonialId">id of the wanted Testimonial</param>
    Task<Testimonial?> GetByTestimonialIdAsync(int testimonialId);
}
