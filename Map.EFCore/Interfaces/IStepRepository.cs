using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;
public interface IStepRepository : IGenericRepository<Step>
{
    /// <summary>
    /// Add a step to a trip at the end
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="step">dto of new</param>
    public Task AddStepLastAsync(Trip trip, Step step);

    /// <summary>
    /// Add a step to a trip before a step
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="nextStep">Step before where to add a new step</param>
    /// <param name="step">new step to add</param>
    public Task AddStepBeforAsync(Trip trip, Step nextStep, Step step);

    /// <summary>
    /// Add a step to a trip after a step
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="previousStep">Step after where to add a new step</param>
    /// <param name="step">new step to add</param>
    public Task AddStepAfterAsync(Trip trip, Step previousStep, Step step);

    /// <summary>
    /// Move a step to the end of the trip
    /// </summary>
    /// <param name="step">step to move</param>
    public Task MoveStepToEndAsync(Step step);

    /// <summary>
    /// Move a step befor another step
    /// </summary>
    /// <param name="step">step to move</param>
    /// <param name="previousStep">step where to move</param>
    Task MoveStepBeforeAsync(Step step, Step previousStep);

    /// <summary>
    /// Move a step after another step
    /// </summary>
    /// <param name="step">step to move</param>
    /// <param name="nextStep">step where to move</param>
    Task MoveStepAfterAsync(Step step, Step nextStep);
}
