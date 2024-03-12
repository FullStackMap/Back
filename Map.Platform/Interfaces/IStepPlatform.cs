using Map.Domain.Entities;
using Map.Domain.Models.Step;

namespace Map.Platform.Interfaces;
public interface IStepPlatform
{
    /// <summary>
    /// Add a step to a trip at the end
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="entity">dto of new</param>
    /// <returns>Step</returns>
    Task AddStepAsync(Trip trip, Step entity);

    /// <summary>
    /// Add a step to a trip before a step
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="nextStep">step where add new step</param>
    /// <param name="entity">dto of new</param>
    Task AddStepBeforAsync(Trip trip, Step nextStep, Step entity);

    /// <summary>
    /// Add a step to a trip after a step
    /// </summary>
    /// <param name="trip">trip where add step</param>
    /// <param name="previousStep">step where add new step</param>
    /// <param name="entity">dto of new</param>
    Task AddStepAfterAsync(Trip trip, Step previousStep, Step entity);

    /// <summary>
    /// Get a step by id
    /// </summary>
    /// <param name="stepId">Id of wanted Step</param>
    /// <returns>step if exist or null</returns>
    Task<Step?> GetStepByIdAsync(Guid stepId);

    /// <summary>
    /// Move a step to the end of the trip
    /// </summary>
    /// <param name="step">step to move</param>
    Task MoveStepToEndAsync(Step step);

    /// <summary>
    /// Move a step before another step
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

    /// <summary>
    /// Delete a step
    /// </summary>
    Task DeleteStepAsync(Step step);

    /// <summary>
    /// Check if a step exist by id
    /// </summary>
    /// <param name="stepId">Id of step to check</param>
    /// <returns>true if exist or false</returns>
    Task<bool> IsStepExistById(Guid stepId);
    Task UpdateStepNameAsync(Step step, UpdateStepNameDto updateStepNameDto);
    Task UpdateStepDescAsync(Step step, UpdateStepDescriptionDto updateStepDescriptionDto);
    Task UpdateStepDateAsync(Step step, UpdateStepDateDto updateStepDateDto);
    Task UpdateStepLocationAsync(Step step, UpdateStepLocationDto updateStepLocationDto);
}
