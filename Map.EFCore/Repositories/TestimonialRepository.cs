using Map.Domain.Entities;
using Map.EFCore.Interfaces;

namespace Map.EFCore.Repositories;
internal class TestimonialRepository : GenericRepository<Testimonial>, ITestimonialRepository
{
    public TestimonialRepository(MapContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task AddTestimonial(MapUser user, Testimonial testimonial)
    {
        user.Testimonials.Add(testimonial);
        await _context.SaveChangesAsync();
    }
}
