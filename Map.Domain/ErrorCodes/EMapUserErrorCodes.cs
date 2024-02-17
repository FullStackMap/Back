namespace Map.Domain.ErrorCodes;
public enum EMapUserErrorCodes
{
    UserIdNotNull = 001,
    UserNotFoundById = 002,
    UserNameNotEmpty = 003,
    EmailNotEmpty = 004,
    EmailNotValid = 005,
    PasswordNotEmpty = 006,
    ConfirmPasswordNotEmpty = 007,
    ConfirmPasswordMustEqualPassword = 008,
    UsernameNotUnique = 009,
    EmailNotUnique = 010,
    UserNotFoundByEmail = 011,
}
