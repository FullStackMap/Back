using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;

public interface ITripRepository : IGenericRepository<Trip>
{
    Task<Trip> UpdateAsync(Trip trip, Trip update);
}