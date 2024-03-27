namespace Map.Domain.Models.Step;
public class StepDtoList
{
    public int StepId { get; set; }
    public Guid TripId { get; set; }
    public int Order { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
