using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class Document
{
    public Guid DocumentId { get; set; }
    public Guid ReservationId { get; set; }
    public virtual Reservation? Reservation { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}
