using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

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

    public virtual IList<Step>? Steps { get; set; }
}
