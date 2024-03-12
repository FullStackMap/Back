using Map.Domain.Entities;
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
    public async Task AddStepBeforAsync(Trip trip, Step nextStep, Step entity)
    {
        await _unitOfWork.Step.AddStepBeforAsync(trip, nextStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepAfterAsync(Trip trip, Step previousStep, Step entity)
    {
        await _unitOfWork.Step.AddStepAfterAsync(trip, previousStep, entity);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task<Step?> GetStepByIdAsync(Guid stepId)
    {
        return await _unitOfWork.Step.GetByIdAsync(stepId);
    }

    /// <inheritdoc/>
    public async Task MoveStepToEndAsync(Step step) => await _unitOfWork.Step.MoveStepToEndAsync(step);

    /// <inheritdoc/>
    public async Task MoveStepBeforeAsync(Step step, Step previousStep) => await _unitOfWork.Step.MoveStepBeforeAsync(step, previousStep);

    /// <inheritdoc/>
    public async Task MoveStepAfterAsync(Step step, Step nextStep) => await _unitOfWork.Step.MoveStepAfterAsync(step, nextStep);

    /// <inheritdoc/>
    public async Task<bool> IsStepExistById(Guid stepId) => await _unitOfWork.Step.IsExistAsync(stepId);

    /// <inheritdoc/>
    public void DeleteStep(Step step) => _unitOfWork.Step.Remove(step);
}
