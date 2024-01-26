using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class Step
{
    public Guid StepId { get; set; }
    public Guid TripId { get; set; }
    public virtual Trip? Trip { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    //TODO: Add Type Table for this
    //public virtual IList<string>? Type { get; set; }
    public virtual IList<TravelTo>? TravelsTo { get; set; }
    public virtual IList<Reservation>? Reservations { get; set; }

}
