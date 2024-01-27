namespace Map.Domain.ErrorCodes;
public enum ETripErrorCodes
{
    TripDtoNull = 101,
    TripNameNotNull = 102,
    TripNameMaxLength = 103,
    TripDescriptionNotNull = 104,
    TripDescriptionMaxLength = 105,
    TripStartDateNotNull = 106,
    TripStartDateNotInPast = 107,
    TripStartDateNotDateOnly = 108,
    TripEndDateNotNull = 109,
    TripEndDateNotDateOnly = 110,
    TripEndDateNotInPast = 111,
    TripEndDateNotBeforStartDate = 112,
    TripEndDateNotEqualStartDate = 113,
    TripNameMinLength = 114,
    TripNameUniqueByUser = 115,
}
