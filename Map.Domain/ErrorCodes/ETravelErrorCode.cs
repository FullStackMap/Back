namespace Map.Domain.ErrorCodes;

public enum ETravelErrorCode
{
    DtoNotNull = 401,
    OriginStepIdNotNull = 402,
    TransportModeNotEmpty = 403,
    DistanceNotEmpty = 404,
    DurationNotEmpty = 405,
    TravelRoadNotEmpty = 406,
    TravelNotEmpty = 407,
    OriginStepIdNotEmpty = 408,
    DestinationStepIdNotEmpty = 409,
    OriginStepOrderAndDestinationStepOrderNotSequential = 410,
    OriginStepIdAndDestinationStepIdNotTheSame = 411,
    OriginStepIdAndDestinationStepIdNotInSameTrip = 412,
}
