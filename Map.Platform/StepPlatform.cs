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
        await _unitOfWork.Step.AddStepLastAsync(trip, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepBeforAsync(Trip trip, Step previousStep, Step entity)
    {
        await _unitOfWork.Travel.RemoveTravelBeforeStepAsync(previousStep);
        await _unitOfWork.Step.AddStepBeforAsync(trip, previousStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepAfterAsync(Trip trip, Step nextStep, Step entity)
    {
        await _unitOfWork.Travel.RemoveTravelAfterStepAsync(nextStep);
        await _unitOfWork.Step.AddStepAfterAsync(trip, nextStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task<Step?> GetStepByIdAsync(int stepId) => await _unitOfWork.Step.GetStepByIdAsync(stepId);

    /// <inheritdoc/>
    public async Task<Step?> GetByStepIdAsync(int stepId) => await _unitOfWork.Step.GetByStepIdAsync(stepId);

    /// <inheritdoc/>
    public async Task MoveStepToEndAsync(Trip trip, Step step)
    {
        await _unitOfWork.Travel.RemoveLinkedTravelAsync(step);
        await _unitOfWork.Step.MoveStepToEndAsync(trip, step);
    }

    /// <inheritdoc/>
    public async Task MoveStepBeforeAsync(Trip trip, Step step, Step previousStep)
    {
        await _unitOfWork.Travel.RemoveLinkedTravelAsync(step);
        await _unitOfWork.Travel.RemoveTravelBeforeStepAsync(previousStep);
        await _unitOfWork.Step.MoveStepBeforeAsync(trip, step, previousStep);
    }

    /// <inheritdoc/>
    public async Task MoveStepAfterAsync(Trip trip, Step step, Step nextStep)
    {
        await _unitOfWork.Travel.RemoveLinkedTravelAsync(step);
        await _unitOfWork.Travel.RemoveTravelAfterStepAsync(nextStep);
        await _unitOfWork.Step.MoveStepAfterAsync(trip, step, nextStep);
    }

    /// <inheritdoc/>
    public async Task<bool> IsStepExistById(Guid stepId) => await _unitOfWork.Step.IsExistAsync(stepId);

    /// <inheritdoc/>
    public async Task DeleteStepAsync(Trip trip, Step step)
    {
        await _unitOfWork.Travel.RemoveLinkedTravelAsync(step);
        await _unitOfWork.Step.MoveStepToEndAsync(trip, step);
        await _unitOfWork.Step.RemoveStepAsync(step);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateStepNameAsync(Step step, UpdateStepNameDto updateStepNameDto)
    {
        step.Name = updateStepNameDto.Name;
        await _unitOfWork.Step.UpdateStepAsync(step);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateStepDescAsync(Step step, UpdateStepDescriptionDto updateStepDescriptionDto)
    {
        step.Description = updateStepDescriptionDto.Description;
        await _unitOfWork.Step.UpdateStepAsync(step);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateStepDateAsync(Step step, UpdateStepDateDto updateStepDateDto)
    {
        step.StartDate = updateStepDateDto.StartDate;
        step.EndDate = updateStepDateDto.EndDate;
        await _unitOfWork.Step.UpdateStepAsync(step);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateStepLocationAsync(Step step, UpdateStepLocationDto updateStepLocationDto)
    {
        step.Latitude = updateStepLocationDto.Latitude;
        step.Longitude = updateStepLocationDto.Longitude;
        await _unitOfWork.Travel.RemoveLinkedTravelAsync(step);
        await _unitOfWork.Step.UpdateStepAsync(step);
        await _unitOfWork.CompleteAsync();
    }
}
