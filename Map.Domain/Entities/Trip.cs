namespace Map.Domain.Entities;

public class Trip
{
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
    public virtual MapUser? User { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? BackgroundPicturePath { get; set; }

    public virtual IList<Step>? Steps { get; set; }
}
