namespace Map.Domain.Entities;

public class Travel
{
    public int TravelId { get; set; }
    public Guid TripId { get; set; }
    public virtual Trip? Trip { get; set; }

    public int OriginStepId { get; set; }
    public int DestinationStepId { get; set; }
    public virtual Step? OriginStep { get; set; }
    public virtual Step? DestinationStep { get; set; }

    public string TransportMode { get; set; }
    public decimal Distance { get; set; }
    public decimal Duration { get; set; }

    public virtual TravelRoad? TravelRoad { get; set; }
}