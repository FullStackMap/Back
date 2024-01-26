﻿using Map.Domain.Entities;
using Map.EFCore.Interfaces;
using Map.Provider.Interfaces;

namespace Map.Provider;
public class TripProvider : ITripProvider
{
    #region Props

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public TripProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    #endregion

    #region PublicMethods

    /// <inheritdoc/>
    public async Task CreateAsync(Trip entity)
    {
        await _unitOfWork.Trip.AddAsync(entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task<Trip?> GetByIdAsync(Guid tripId) => await _unitOfWork.Trip.GetByIdAsync(tripId);

    /// <inheritdoc/>
    public async Task<IList<Trip>> GetAllAsync() => await _unitOfWork.Trip.GetAllAsync();

    /// <inheritdoc/>
    public async Task<Trip> UpdateAsync(Trip trip, Trip update)
    {
        trip = await _unitOfWork.Trip.UpdateAsync(trip, update);
        await _unitOfWork.CompleteAsync();
        return trip;
    }

    /// <inheritdoc/>
    public void Delete(Trip entity)
    {
        _unitOfWork.Trip.Remove(entity);
        _unitOfWork.Complete();
    }
    #endregion
}
