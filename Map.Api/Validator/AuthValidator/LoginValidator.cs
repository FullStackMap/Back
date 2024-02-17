using FluentValidation;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AuthDto;

namespace Map.API.Validator.AuthValidator;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(EAuthErrorCodes.DtoNotNull))
            .WithMessage("Dto is required");

        #region Username
        RuleFor(dto => dto.Username)
            .NotNull()
            .WithErrorCode(nameof(EMapUserErrorCodes.UserNameNotEmpty))
            .WithMessage("Username is required");

        #endregion

        #region Password

        RuleFor(dto => dto.Password)
            .NotNull()
            .WithErrorCode(nameof(EMapUserErrorCodes.PasswordNotEmpty))
            .WithMessage("Password is required");

        #endregion
    }
}
