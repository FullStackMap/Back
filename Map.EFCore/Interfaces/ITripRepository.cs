using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;

public interface ITripRepository : IGenericRepository<Trip>
{
    /// <summary>
    /// Get all trips asynchronously where user id is equal to the given user id
    /// </summary>
    /// <param name="UserId">id of user</param>
    /// <returns>List of trips</returns>
    Task<List<Trip>?> GetAllWhereUserId(Guid UserId);

    /// <summary>
    /// Update a trip entity asynchronously
    /// </summary>
    /// <param name="trip">entity</param>
    /// <param name="update">update of entity</param>
    /// <returns>new trip version</returns>
    Task<Trip> UpdateAsync(Trip trip, Trip update);
}