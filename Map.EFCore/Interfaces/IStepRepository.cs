using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;
public interface IStepRepository : IGenericRepository<Step>
{
    /// <summary>
    /// Add step on last position of trip
    /// </summary>
    /// <param name="trip">trip</param>
    /// <param name="step">step</param>
    public Task AddStepLast(Trip trip, Step step);

    /// <summary>
    /// Add step befor a step
    /// </summary>
    /// <param name="trip">trip</param>
    /// <param name="nextStep">nextStep</param>
    /// <param name="step">step</param>
    public Task AddStepBefor(Trip trip, Step nextStep, Step step);

    /// <summary>
    /// Add step after a step
    /// </summary>
    /// <param name="trip">trip</param>
    /// <param name="previousStep">previousStep</param>
    /// <param name="step">step</param>
    public Task AddStepAfter(Trip trip, Step previousStep, Step step);
}
