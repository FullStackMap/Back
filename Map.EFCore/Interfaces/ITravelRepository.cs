using Map.Domain.Entities;

namespace Map.EFCore.Interfaces;
public interface ITravelRepository : IGenericRepository<Travel>
{
    /// <summary>
    /// Add travel after step
    /// </summary>
    /// <param name="step">Step</param>
    /// <param name="travel">Travel</param>
    public Task AddTravelBeforeAsync(Step step, Travel travel);

    /// <summary>
    /// Add linked travel (Before and After) for step
    /// </summary>
    /// <param name="step">Step</param>
    /// <param name="travelAfter">Travel after</param>
    /// <param name="travelBefore">Travel before</param>
    public Task AddLinkedTravelAsync(Step step, Travel travelBefore, Travel travelAfter);

    /// <summary>
    /// Remove TravelBefore step
    /// </summary>
    /// <param name="step">Step</param>
    public Task RemoveTravelBeforeStepAsync(Step step);

    /// <summary>
    /// Remove TravelAfter step
    /// </summary>
    /// <param name="step">Step</param>
    public Task RemoveTravelAfterStepAsync(Step step);

    /// <summary>
    /// Remove linked travel (Before and After) for step
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public Task RemoveLinkedTravelAsync(Step step);
}
