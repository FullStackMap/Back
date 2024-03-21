namespace Map.Domain.Entities;

public class Step
{
    public int StepId { get; set; }

    public Guid TripId { get; set; }
    public virtual Trip? Trip { get; set; }

    public string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public virtual Travel? TravelBefore { get; set; }
    public virtual Travel? TravelAfter { get; set; }

}
