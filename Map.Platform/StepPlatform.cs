using Map.Domain.Entities;
using Map.Domain.Models.Step;
using Map.EFCore.Interfaces;
using Map.Platform.Interfaces;

namespace Map.Platform;
public class StepPlatform : IStepPlatform
{
    #region Props
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Ctor
    public StepPlatform(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    #endregion

    /// <inheritdoc/>
    public async Task AddStepAsync(Trip trip, Step entity)
    {
        await _unitOfWork.Step.AddStepLast(trip, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepBeforAsync(Trip trip, Step nextStep, Step entity)
    {
        await _unitOfWork.Step.AddStepBefor(trip, nextStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepAfterAsync(Trip trip, Step previousStep, Step entity)
    {
        await _unitOfWork.Step.AddStepAfter(trip, previousStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task<Step?> GetStepByIdAsync(Guid stepId)
    {
        return await _unitOfWork.Step.GetByIdAsync(stepId);
    }

    public Task<bool> DeleteStepAsync(Guid stepId) => throw new NotImplementedException();
    public Task<Step?> GetStepByTripIdAndStepNumberAsync(Guid tripId, int stepNumber) => throw new NotImplementedException();
    public Task<ICollection<Step>> GetStepsByTripIdAsync(Guid tripId) => throw new NotImplementedException();
    public Task<Step?> UpdateStepAsync(Guid stepId, AddStepDto addStepDto) => throw new NotImplementedException();
}
