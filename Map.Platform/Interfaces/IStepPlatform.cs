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
    /// Add a step to a trip befor a step
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

    Task<Step?> GetStepByTripIdAndStepNumberAsync(Guid tripId, int stepNumber);
    Task<ICollection<Step>> GetStepsByTripIdAsync(Guid tripId);
    Task<Step?> UpdateStepAsync(Guid stepId, AddStepDto addStepDto);
    Task<bool> DeleteStepAsync(Guid stepId);
}
