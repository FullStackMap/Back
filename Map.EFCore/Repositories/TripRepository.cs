using Map.Domain.Entities;
using Map.Domain.Models.Trip;
using Map.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Map.EFCore.Repositories;
public class TripRepository : GenericRepository<Trip>, ITripRepository
{
    public TripRepository(MapContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public Task<List<Trip>> GetAllWhereUserId(Guid UserId) => _context.Trip.Where(t => t.UserId == UserId).ToListAsync();

    /// <inheritdoc/>
    public async Task<Trip?> GetTripByIdAsync(Guid TripId) => await _context.Trip.Include(t => t.Steps).FirstOrDefaultAsync(t => t.TripId == TripId);

    /// <inheritdoc/>
    public async Task<Trip> UpdateAsync(Trip trip, UpdateTripDto update)
    {
        trip.UserId = update.UserId;
        trip.Name = update.Name;
        trip.Description = update.Description;
        trip.StartDate = update.StartDate;
        trip.EndDate = update.EndDate;
        trip.BackgroundPicturePath = update.BackgroundPicturePath;

        await _context.SaveChangesAsync();
        return trip;
    }
}
