namespace Map.Domain.Entities;
public class CoordinateStepTravelToAssociation
{
    public Guid CoordinateId { get; set; }
    public Coordinate Coordinate { get; set; }

    public Guid? StepId { get; set; }
    public Step Step { get; set; }

    public Guid? TravelToId { get; set; }
    public TravelTo TravelTo { get; set; }
}
