﻿using Map.Domain.Entities;
using Map.Domain.Models.Trip;

namespace Map.Platform.Interfaces;
public interface ITripPlatform
{
    /// <summary>
    /// Create a new trip asynchronously.
    /// </summary>
    /// <param name="trip">Entity to create</param>
    /// <returns>Trip</returns>
    public Task AddTripAsync(Trip entity);

    /// <summary>
    /// Get one trip asynchronously by id
    /// </summary>
    /// <param name="tripId">Id of the wanted trip</param>
    /// <returns>trip or null</returns>
    public Task<Trip?> GetTripByIdAsync(Guid tripId);

    /// <summary>
    /// Get Queryable of all trips
    /// </summary>
    IQueryable<Trip> GetAll();

    /// <summary>
    /// Get all trips asynchronously by user id
    /// </summary>
    /// <param name="userId">Id of user</param>
    /// <returns>List of trip or null</returns>
    Task<List<Trip>?> GetTripListByUserIdAsync(Guid userId);

    /// <summary>
    /// Update a trip asynchronously
    /// </summary>
    /// <param name="entity">A trip to update</param>
    /// <returns>Updated Trip</returns>
    public Task<Trip> UpdateTripAsync(Trip entity, UpdateTripDto update);

    /// <summary>
    /// Delete a trip asynchronously
    /// </summary>
    /// <param name="entity">entity to delete</param>
    /// <returns>Task</returns>
    public void Delete(Trip entity);
}
