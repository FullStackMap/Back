namespace Map.API.Models.TripDto;

/// <summary>
/// Trip data transfer object.
/// </summary>
public class TripDto
{
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    //Todo : Add steps
    //public IList<Step>? Steps { get; set; }
}
