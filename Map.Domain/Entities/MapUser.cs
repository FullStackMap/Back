using Microsoft.AspNetCore.Identity;

namespace Map.Domain.Entities;

public class MapUser : IdentityUser<Guid>
{
    public virtual IList<Trip>? Trips { get; set; }
    public virtual IList<Testimonial>? Testimonials { get; set; }
}
