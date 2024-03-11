using Map.Domain.Entities;
using Map.EFCore.Interfaces;

namespace Map.EFCore.Repositories;
public class StepRepository : GenericRepository<Step>, IStepRepository
{
    public StepRepository(MapContext context) : base(context)
    {
    }
    /// <inheritdoc/>
    public Task AddStepLast(Trip trip, Step step)
    {
        int lastStepNumber = trip.Steps.Last().StepNumber;
        step.StepNumber = lastStepNumber++;

        _context.Step.Add(step);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task AddStepAfter(Trip trip, Step previousStep, Step step)
    {
        int previousStepIndex = trip.Steps.ToList().FindIndex(s => s.StepNumber == previousStep.StepNumber);

        if (previousStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {previousStep},  n'as pas été trouver dans le trip : {trip}");

        for (int i = previousStepIndex + 1; i < trip.Steps.Count; i++)
        {
            trip.Steps[i].StepNumber++;
        }

        step.StepNumber++;

        _context.Step.Add(step);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task AddStepBefor(Trip trip, Step nextStep, Step step)
    {
        int nextStepIndex = trip.Steps.ToList().FindIndex(s => s.StepNumber == nextStep.StepNumber);

        if (nextStepIndex is -1)
            throw new ArgumentException($"L'étape suivante : {nextStep},  n'as pas été trouver dans le trip : {trip}");

        for (int i = nextStepIndex; i < trip.Steps.Count; i++)
        {
            trip.Steps[i].StepNumber++;
        }

        step.StepNumber++;

        _context.Step.Add(step);
        return _context.SaveChangesAsync();
    }
}
