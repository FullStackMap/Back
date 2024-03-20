using Map.Domain.Entities;
using Map.EFCore.Interfaces;

namespace Map.EFCore.Repositories;
public class StepRepository : GenericRepository<Step>, IStepRepository
{
    public StepRepository(MapContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public Task AddStepAfterAsync(Trip trip, Step previousStep, Step step) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task AddStepBeforAsync(Trip trip, Step nextStep, Step step) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task AddStepLastAsync(Trip trip, Step step) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task MoveStepAfterAsync(Step step, Step nextStep) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task MoveStepBeforeAsync(Step step, Step previousStep) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task MoveStepToEndAsync(Step step) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task RemoveStepAsync(Step step) => throw new NotImplementedException();
    /// <inheritdoc/>
    public Task UpdateStepAsync(Step step) => throw new NotImplementedException();
}
