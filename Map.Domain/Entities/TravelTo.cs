using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class TravelTo
{
    public Guid TravelToId { get; set; }
    public Guid PreviousStepId { get; set; }
    public virtual Step? PreviousStep { get; set; }
    public Guid CurrentStepId { get; set; }
    public virtual Step? CurrentStep { get; set; }
    public string? TransportMode { get; set; }

    //public virtual IList<WayPath>? WayPaths { get; set; }
    public decimal Distance { get; set; }
    public decimal? Price { get; set; }
    public int? CarbonEmition { get; set; }
}
