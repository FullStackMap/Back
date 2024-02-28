using FluentValidation;
using Map.API.Extension;
using Map.Domain.ErrorCodes;
using Map.Domain.Models.AuthDto;

namespace Map.API.Validator.AuthValidator;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(EAuthErrorCodes.DtoNotNull.ToStringValue())
            .WithMessage("Dto is required");

        #region Username
        RuleFor(dto => dto.Username)
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.UserNameNotEmpty.ToStringValue())
            .WithMessage("Username is required");

        #endregion

        #region Password

        RuleFor(dto => dto.Password)
            .NotNull()
            .WithErrorCode(EMapUserErrorCodes.PasswordNotEmpty.ToStringValue())
            .WithMessage("Password is required");

        #endregion
    }
}
