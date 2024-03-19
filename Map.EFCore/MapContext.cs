using Map.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Map.EFCore;

public class MapContext : IdentityDbContext<MapUser, IdentityRole<Guid>, Guid>
{
    #region Properties

    public DbSet<MapUser> MapUser { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<Step> Step { get; set; }
    public DbSet<Travel> Travel { get; set; }
    public DbSet<TravelRoad> TravelRoad { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<Document> Document { get; set; }

    #endregion Properties

    #region Constructor

    public MapContext()
    { }

    public MapContext(DbContextOptions<MapContext> options)
        : base(options)
    { }

    #endregion Constructor

    #region Method

    /// <summary>
    /// Ons the model creating.
    /// </summary>
    /// <param name="builder">The builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MapUser>(u =>
        {
            u.ToTable(name: "MapUsers");
            u.HasKey(u => u.Id);

            u.Property(u => u.UserName).IsRequired();
            u.Property(u => u.NormalizedUserName).IsRequired();
            u.Property(u => u.Email).IsRequired();
            u.Property(u => u.NormalizedEmail).IsRequired();

            u.HasMany(u => u.Trips)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        });

        builder.Entity<Trip>(t =>
        {
            t.ToTable(name: "Trips");
            t.HasKey(t => t.TripId);

            t.Property(t => t.Name).IsRequired();
            t.Property(t => t.Description).HasMaxLength(500);
            t.Property(t => t.StartDate).IsRequired();
            t.Property(t => t.EndDate).IsRequired();
            t.Property(t => t.BackgroundPicturePath).IsRequired();

            t.HasMany(t => t.Steps)
                .WithOne(s => s.Trip)
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Step>(s =>
        {
            s.ToTable(name: "Steps");
            s.HasKey(s => s.StepId);

            s.Property(s => s.StepNumber).IsRequired();
            s.Property(s => s.TripId).IsRequired();
            s.Property(s => s.Name).IsRequired();
            s.Property(s => s.Description).HasMaxLength(500);
            s.Property(s => s.StartDate);
            s.Property(s => s.EndDate);
            s.Property(s => s.Latitude).HasPrecision(18, 12).IsRequired();
            s.Property(s => s.Longitude).HasPrecision(18, 12).IsRequired();

            s.HasMany(s => s.Reservations)
                .WithOne(r => r.Step)
                .HasForeignKey(r => r.StepId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Travel>(t =>
        {
            t.ToTable("Travels");
            t.HasKey(t => t.TravelId);

            t.Property(t => t.TransportMode).IsRequired();
            t.Property(t => t.Distance).IsRequired();
            t.Property(t => t.Duration).IsRequired();
            t.Property(t => t.CarbonEmition);
            t.Property(t => t.OriginStepId).IsRequired();
            t.Property(t => t.DestinationStepId).IsRequired();

            t.HasOne(t => t.OriginStep)
                .WithOne(t => t.TravelBefore)
                .HasForeignKey<Travel>(t => t.OriginStepId)
                .OnDelete(DeleteBehavior.Restrict)
;

            t.HasOne(t => t.DestinationStep)
                .WithOne(t => t.TravelAfter)
                .HasForeignKey<Travel>(t => t.DestinationStepId)
                .OnDelete(DeleteBehavior.Restrict);

        });

        builder.Entity<TravelRoad>(tr =>
        {
            tr.ToTable("TravelRoads");
            tr.HasKey(tr => tr.TravelId);

            tr.Property(tr => tr.RoadCoordinates).HasMaxLength(-1).IsRequired();

            tr.HasOne(tr => tr.Travel)
                .WithOne(tr => tr.TravelRoad)
                .HasForeignKey<Travel>(t => t.TravelId)
                .OnDelete(DeleteBehavior.Cascade);
        });




        builder.Entity<Reservation>(r =>
        {
            r.ToTable("Reservations");
            r.HasKey(r => r.ReservationId);

            r.Property(r => r.Name).IsRequired();
            r.Property(r => r.CompanieName);
            r.Property(r => r.IsReservated).IsRequired();
            r.Property(r => r.TransportNumber);
            r.Property(r => r.PlaceCount);
            r.Property(r => r.Terminal);
            r.Property(r => r.TerminaleGate);
            r.Property(r => r.VehiculeType);
            r.Property(r => r.StartTime).IsRequired();
            r.Property(r => r.EndTime);
            r.Property(r => r.StartTimeGMT);
            r.Property(r => r.EndTimeGMT);
            r.Property(r => r.StartLatitude);
            r.Property(r => r.StartLongitude);
            r.Property(r => r.EndLatitude);
            r.Property(r => r.EndLongitude);
            r.Property(r => r.Note);
            r.Property(r => r.Price);
            r.Property(r => r.Currency);
            r.Property(r => r.ReservationNumber);
            r.Property(r => r.ReservationLastName);
            r.Property(r => r.ReservationUrl);
            r.Property(r => r.ReservationPhoneNumber);
            r.Property(r => r.ReservationEmail);

            r.HasMany(r => r.Documents)
                .WithOne(d => d.Reservation)
                .HasForeignKey(d => d.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<Document>(d =>
        {
            d.ToTable(name: "Documents");
            d.HasKey(d => d.DocumentId);

            d.Property(d => d.Name).IsRequired();
            d.Property(d => d.Path).IsRequired();
        });


    }

    #endregion Method
}