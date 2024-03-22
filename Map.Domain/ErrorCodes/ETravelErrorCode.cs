namespace Map.Domain.ErrorCodes;

public enum ETravelErrorCode
{
    DtoNotNull = 401,
    OriginStepIdNotNull = 402,
    TransportModeNotEmpty = 403,
    DistanceNotEmpty = 404,
    DurationNotEmpty = 405,
    TravelRoadNotEmpty = 406,
}
