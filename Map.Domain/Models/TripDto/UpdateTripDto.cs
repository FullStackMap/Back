namespace Map.Domain.Models.TripDto;

/// <summary>
/// Dto for udpate a trip.
/// </summary>
public class UpdateTripDto
{
    /// <summary>
    /// Id of the user who created the trip.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Name of the trip.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the trip.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Start date of the trip.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the trip.
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Path of the background picture.
    /// </summary>
    public string? BackgroundPicturePath { get; set; }
}
