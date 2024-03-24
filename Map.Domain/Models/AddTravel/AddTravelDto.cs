namespace Map.Domain.Models.AddTravel;
public class AddTravelDto
{
    public string TransportMode { get; set; }
    public decimal Distance { get; set; }
    public decimal Duration { get; set; }
    public int OriginStepId { get; set; }
    public int DestinationStepId { get; set; }

    /// <summary>
    /// JsonString of All Coordinates of Travel
    /// </summary>
    public string TravelRoad { get; set; }
}
