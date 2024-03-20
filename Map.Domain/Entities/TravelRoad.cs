namespace Map.Domain.Entities;
public class TravelRoad
{
    public int TravelId { get; set; }
    public virtual Travel? Travel { get; set; }

    public string RoadCoordinates { get; set; }
}
