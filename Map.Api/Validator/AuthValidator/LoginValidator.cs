using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.Auth;

namespace Map.API.Validator.AuthValidator;

internal class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(EAuthErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Dto is required");

        #region Email
        RuleFor(dto => dto.Email)
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.UserNameNotEmpty.ToStringValue())
            .WithMessage("Email is required")
            .EmailAddress()
            .WithErrorCode(EMapUserErrorCodes.EmailNotValid.ToStringValue())
            .WithMessage("Email must be valid");

        #endregion

        #region Password

        RuleFor(dto => dto.Password)
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.PasswordNotEmpty.ToStringValue())
            .WithMessage("Password is required");

        #endregion
    }
}
