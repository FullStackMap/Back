namespace Map.Domain.ErrorCodes;

public enum EStepErrorCodes
{
    DtoNotNull = 301,
    NameNotEmpty = 302,
    LatitudeNotEmpty = 303,
    LongitudeNotEmpty = 304,
    DescriptionMaxLength = 305,
    NameMinLength = 306,
    NameMaxLength = 307,
    StepNotFoundById = 308,
    StepNotDeleted = 309,
    StepsNotInSameTrip = 310,
    StepNumberNotInOrder = 311,
}
