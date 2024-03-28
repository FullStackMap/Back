using Map.Domain.Entities;
using Map.EFCore.Interfaces;
using Map.Platform.Interfaces;

namespace Map.Platform;

internal class TravelPlatform : ITravelPlatform
{
    private readonly IUnitOfWork _unitOfWork;

    public TravelPlatform(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <inheritdoc/>
    public async Task AddTravelBetweenStepsAsync(Step origin, Step destination, Travel travel)
    {
        travel.TripId = origin.TripId;
        await _unitOfWork.Travel.RemoveTravelAfterStepAsync(origin);
        await _unitOfWork.Travel.RemoveTravelBeforeStepAsync(destination);
        await _unitOfWork.Travel.AddAsync(travel);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveTravelBeforeStepAsync(Step step) => await _unitOfWork.Travel.RemoveTravelBeforeStepAsync(step);

    /// <inheritdoc/>
    public async Task RemoveTravelAfterStepAsync(Step step) => await _unitOfWork.Travel.RemoveTravelAfterStepAsync(step);
}
