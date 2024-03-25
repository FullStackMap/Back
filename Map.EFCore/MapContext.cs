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
    public DbSet<Testimonial> Testimonial { get; set; }

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

        });

        builder.Entity<Trip>(t =>
        {
            t.ToTable(name: "Trips");
            t.HasKey(t => t.TripId);

            t.Property(t => t.UserId).IsRequired();
            t.Property(t => t.Name).IsRequired();
            t.Property(t => t.StartDate).IsRequired();
            t.Property(t => t.EndDate).IsRequired();
            t.Property(t => t.BackgroundPicturePath).IsRequired();

            t.Property(t => t.Description).IsRequired(false).HasMaxLength(500);

            t.HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Step>(s =>
        {
            s.ToTable(name: "Steps");
            s.HasKey(s => s.StepId);

            s.Property(s => s.TripId).IsRequired();
            s.Property(s => s.Name).IsRequired();
            s.Property(s => s.Latitude).HasPrecision(18, 12).IsRequired();
            s.Property(s => s.Longitude).HasPrecision(18, 12).IsRequired();

            s.Property(s => s.Description).HasMaxLength(500);
            s.Property(s => s.StartDate).IsRequired(false);
            s.Property(s => s.EndDate).IsRequired(false);


            s.HasOne(s => s.Trip)
                .WithMany(t => t.Steps)
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            s.HasOne(s => s.TravelBefore)
                .WithOne(t => t.DestinationStep)
                .HasForeignKey<Travel>(t => t.DestinationStepId)
                .OnDelete(DeleteBehavior.SetNull);

            s.HasOne(s => s.TravelAfter)
                .WithOne(t => t.OriginStep)
                .HasForeignKey<Travel>(t => t.OriginStepId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Travel>(t =>
        {
            t.ToTable("Travels");
            t.HasKey(t => t.TravelId);

            t.Property(t => t.OriginStepId).IsRequired();
            t.Property(t => t.DestinationStepId).IsRequired();
            t.Property(t => t.TransportMode).IsRequired();
            t.Property(t => t.Distance).HasPrecision(18, 12).IsRequired();
            t.Property(t => t.Duration).HasPrecision(18, 12).IsRequired();

            t.HasOne(t => t.OriginStep)
                .WithOne(s => s.TravelAfter)
                .HasForeignKey<Travel>(s => s.OriginStepId)
                .OnDelete(DeleteBehavior.Restrict)
;
            t.HasOne(t => t.DestinationStep)
                .WithOne(s => s.TravelBefore)
                .HasForeignKey<Travel>(s => s.DestinationStepId)
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

        builder.Entity<Testimonial>(t =>
        {
            t.ToTable("Testimonials");
            t.HasKey(t => t.TestimonialId);

            t.Property(t => t.FeedBack).IsRequired().HasMaxLength(500);
            t.Property(t => t.UserId).IsRequired();
            t.Property(t => t.rate).IsRequired();
            t.Property(t => t.TestimonialDate).IsRequired();

            t.HasOne(t => t.User)
                .WithMany(u => u.Testimonials)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }

    #endregion Method
}