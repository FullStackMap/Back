using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Testimonial;

namespace Map.API.Validator.TestimonialValidator;

public class AddTestimonialValidator : AbstractValidator<AddTestimonialDto>
{
    public AddTestimonialValidator()
    {
        RuleFor(dto => dto)
            //Check if the dto is not empty
            .NotEmpty()
            .WithErrorCode(ETestimonialErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Le Dto est requis");

        #region FeedBack
        RuleFor(dto => dto.FeedBack)
            //Check if the feedback is not empty
            .NotEmpty()
            .WithErrorCode(ETestimonialErrorCodes.FeedBackNotEmpty.ToStringValue())
            .WithMessage("Le feedback est requis")

            //check if the feedback is less than 500 characters
            .MaximumLength(500)
            .WithErrorCode(ETestimonialErrorCodes.FeedBackMaxLength.ToStringValue())
            .WithMessage("Le feedback doit contenir moins de 500 caractères")

            //chec if the feedback is more than 10 characters
            .MinimumLength(10)
            .WithErrorCode(ETestimonialErrorCodes.FeedBackMinLength.ToStringValue())
            .WithMessage("Le feedback doit contenir plus de 10 caractères");
        #endregion

        #region Rate
        RuleFor(dto => dto.rate)
            //check if the rate value is not empty
            .NotEmpty()
            .WithErrorCode(ETestimonialErrorCodes.RateNotNull.ToStringValue())
            .WithMessage("La note est requise")

            //Check if the rate is more than 0
            .GreaterThan(0)
            .WithErrorCode(ETestimonialErrorCodes.RateMoreThanOne.ToStringValue())
            .WithMessage("La note doit être comprise entre 1 et 5")

            //check if the rate is less than 6
            .LessThan(6)
            .WithErrorCode(ETestimonialErrorCodes.RateLessThanSix.ToStringValue())
            .WithMessage("La note doit être comprise entre 1 et 5");
        #endregion

        #region Date
        RuleFor(dto => dto.TestimonialDate)
            //Check if the date is not null
            .NotEmpty()
            .WithErrorCode(ETestimonialErrorCodes.DateNotNull.ToStringValue())
            .WithMessage("La date est requise");
        #endregion
    }
}
