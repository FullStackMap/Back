namespace Map.Domain.Models.Testimonial;
public class AddTestimonialDto
{
    public string FeedBack { get; set; }
    public int rate { get; set; }
    public DateOnly TestimonialDate { get; set; }
}
