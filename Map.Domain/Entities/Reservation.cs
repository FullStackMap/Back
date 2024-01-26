using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class Reservation
{
    public Guid ReservationId { get; set; }
    public Guid StepId { get; set; }
    public virtual Step? Step { get; set; }

    public string? Name { get; set; }
    public string? CompanieName { get; set; }
    public bool IsReservated { get; set; }
    public string? TransportNumber { get; set; }
    public int? PlaceCount { get; set; }

    public string? Terminal { get; set; }
    public string? TerminaleGate { get; set; }

    public string? VehiculeType { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? StartTimeGMT { get; set; }
    public string? EndTimeGMT { get; set;}
    public decimal? StartLatitude { get; set; }
    public decimal? StartLongitude { get; set; }
    public decimal? EndLatitude { get; set; }
    public decimal? EndLongitude { get; set; }

    public string? Note { get; set; }
    public virtual IList<Document>? Documents { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public string? ReservationNumber { get; set; }
    public string? ReservationLastName { get; set; }
    public string? ReservationUrl { get; set; }
    public string? ReservationPhoneNumber { get; set; }
    public string? ReservationEmail { get; set; }

}
