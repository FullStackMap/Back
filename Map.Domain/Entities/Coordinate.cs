namespace Map.Domain.Entities;
public class Coordinate
{
    public Guid CoordinateId { get; set; }
    public int Index { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public CoordinateStepTravelToAssociation CoordinateStepTravelToAssociation { get; set; }
}
