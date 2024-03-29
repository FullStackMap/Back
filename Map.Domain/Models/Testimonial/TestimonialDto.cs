﻿using Map.Domain.Models.Auth;

namespace Map.Domain.Models.Testimonial;
public class TestimonialDto
{
    public int TestimonialId { get; set; }
    public string FeedBack { get; set; }

    public Guid UserId { get; set; }
    public virtual MapUserDto? User { get; set; }

    public int rate { get; set; }
    public DateOnly TestimonialDate { get; set; }
}
