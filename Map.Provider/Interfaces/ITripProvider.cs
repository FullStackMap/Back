using Map.Domain.Entities;

namespace Map.Provider.Interfaces;
public interface ITripProvider
{
    /// <summary>
    /// Create a new trip asynchronously.
    /// </summary>
    /// <param name="trip">Entity to create</param>
    /// <returns>Trip</returns>
    public Task CreateAsync(Trip entity);

    /// <summary>
    /// Get one trip asynchronously by id
    /// </summary>
    /// <param name="tripId">Id of the wanted trip</param>
    /// <returns>trip or null</returns>
    public Task<Trip?> GetByIdAsync(Guid tripId);

    /// <summary>
    /// Get all trips asynchronously
    /// </summary>
    /// <returns>List of trips</returns>
    public Task<IList<Trip>> GetAllAsync();

    /// <summary>
    /// Update a trip asynchronously
    /// </summary>
    /// <param name="trip">A trip to update</param>
    /// <returns>Updated Trip</returns>
    public Task<Trip> UpdateAsync(Trip entity, Trip update);

    /// <summary>
    /// Delete a trip asynchronously
    /// </summary>
    /// <param name="Trip">entity to delete</param>
    /// <returns>Task</returns>
    public void Delete(Trip entity);
}
