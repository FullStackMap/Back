using Map.Domain.Entities;
using Map.EFCore.Interfaces;

namespace Map.EFCore.Repositories;
public class StepRepository : GenericRepository<Step>, IStepRepository
{
    public StepRepository(MapContext context) : base(context)
    {
    }
    /// <inheritdoc/>
    public Task AddStepLastAsync(Trip trip, Step step)
    {
        int lastStepNumber = trip.Steps.Last().StepNumber;
        step.StepNumber = lastStepNumber++;

        _context.Step.Add(step);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task AddStepAfterAsync(Trip trip, Step previousStep, Step step)
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
    public Task AddStepBeforAsync(Trip trip, Step nextStep, Step step)
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

    /// <inheritdoc/>
    public Task MoveStepToEndAsync(Step step)
    {
        Trip trip = step.Trip;
        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int lastStepNumber = trip.Steps.Last().StepNumber;

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        for (int i = stepIndex + 1; i < trip.Steps.Count; i++)
            trip.Steps[i].StepNumber--;

        step.StepNumber = lastStepNumber;

        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task MoveStepBeforeAsync(Step step, Step previousStep)
    {
        Trip trip = step.Trip;
        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int previousStepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == previousStep.StepId);

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        if (previousStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {previousStep},  n'as pas été trouver dans le trip : {trip}");

        step.StepNumber = previousStep.StepNumber;

        for (int i = previousStepIndex; i < trip.Steps.Count; i++)
            trip.Steps[i].StepNumber++;

        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task MoveStepAfterAsync(Step step, Step nextStep)
    {
        Trip trip = step.Trip;
        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int nextStepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == nextStep.StepId);

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        if (nextStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {nextStep},  n'as pas été trouver dans le trip : {trip}");

        step.StepNumber = nextStep.StepNumber;

        for (int i = nextStepIndex + 1; i < trip.Steps.Count; i++)
            trip.Steps[i].StepNumber++;

        return _context.SaveChangesAsync();
    }
}
