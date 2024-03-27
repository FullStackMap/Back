namespace Map.Domain.Models.Travels;
public class TravelDtoList
{
    public int TravelId { get; set; }

    public int OriginStepId { get; set; }
    public int DestinationStepId { get; set; }

    public string TransportMode { get; set; }
    public decimal Distance { get; set; }
    public decimal Duration { get; set; }

    public virtual TravelRoadDto? TravelRoad { get; set; }
}
