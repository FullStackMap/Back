using Map.Domain.Models.Trip;

namespace Map.Domain.Models.Step;

public class StepDto
{
    public int StepId { get; set; }
    public Guid TripId { get; set; }
    public int Order { get; set; }
    public virtual TripDto? Trip { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    //TODO update this when TravelTo and Reservation are implemented
    //public virtual IList<TravelTo>? TravelsTo { get; set; }
    //public virtual IList<Reservation>? Reservations { get; set; }
}
