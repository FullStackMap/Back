namespace Map.Domain.Entities;

public class TravelTo
{
    public Guid TravelToId { get; set; }
    public Guid PreviousStepId { get; set; }
    public virtual Step? PreviousStep { get; set; }
    public Guid CurrentStepId { get; set; }
    public virtual Step? CurrentStep { get; set; }
    public string? TransportMode { get; set; }

    public decimal Distance { get; set; }
    public decimal Duration { get; set; }
    public int? CarbonEmition { get; set; }

    public IList<CoordinateStepTravelToAssociation> CoordinateStepTravelToAssociations { get; set; }
}