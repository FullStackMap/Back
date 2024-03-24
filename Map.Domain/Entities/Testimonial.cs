namespace Map.Domain.Entities;
public class Testimonial
{
    public int TestimonialId { get; set; }
    public string FeedBack { get; set; }

    public Guid UserId { get; set; }
    public virtual MapUser? User { get; set; }

    public int rate { get; set; }
    public DateOnly TestimonialDate { get; set; }
}
