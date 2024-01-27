using Map.Domain.Entities;
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
    public async Task<Trip> UpdateAsync(Trip trip, Trip update)
    {
        trip.UserId = update.UserId;
        trip.User = update.User;
        trip.Name = update.Name;
        trip.Description = update.Description;
        trip.StartDate = update.StartDate;
        trip.EndDate = update.EndDate;

        await _context.SaveChangesAsync();
        return trip;
    }
}
