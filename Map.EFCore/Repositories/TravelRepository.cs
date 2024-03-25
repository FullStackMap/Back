using Map.Domain.Entities;
using Map.EFCore.Interfaces;

namespace Map.EFCore.Repositories;
public class TravelRepository : GenericRepository<Travel>, ITravelRepository
{
    public TravelRepository(MapContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public Task AddLinkedTravelAsync(Step step, Travel travelBefore, Travel travelAfter)
    {
        step.TravelBefore = travelBefore;
        step.TravelAfter = travelAfter;

        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task AddTravelBeforeAsync(Step step, Travel travel)
    {
        step.TravelBefore = travel;

        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task RemoveLinkedTravelAsync(Step step)
    {
        if (step.TravelBefore is not null)
            _context.Travel.Remove(step.TravelBefore);

        if (step.TravelAfter is not null)
            _context.Travel.Remove(step.TravelAfter);

        return _context.SaveChangesAsync();

    }

    /// <inheritdoc/>
    public Task RemoveTravelAfterStepAsync(Step step)
    {
        if (step.TravelAfter is null)
            return Task.CompletedTask;
        _context.Travel.Remove(step.TravelAfter);

        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task RemoveTravelBeforeStepAsync(Step step)
    {
        if (step.TravelBefore is null)
            return Task.CompletedTask;
        _context.Travel.Remove(step.TravelBefore);

        return _context.SaveChangesAsync();
    }
}
