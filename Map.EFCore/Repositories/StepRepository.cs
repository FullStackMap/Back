using Map.Domain.Entities;
using Map.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Map.EFCore.Repositories;
public class StepRepository : GenericRepository<Step>, IStepRepository
{
    public StepRepository(MapContext context) : base(context)
    {
    }
    /// <inheritdoc/>
    public async Task AddStepLastAsync(Trip trip, Step step)
    {
        int lastOrder = trip.Steps.Any() ? trip.Steps.Last().Order : 0;
        step.Order = lastOrder + 1;

        trip.Steps.Add(step);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepAfterAsync(Trip trip, Step nextStep, Step step)
    {
        trip.Steps = trip.Steps.OrderBy(step => step.Order).ToList();
        int nextStepIndex = trip.Steps.ToList().FindIndex(s => s.Order == nextStep.Order);

        if (nextStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {nextStep},  n'as pas été trouver dans le trip : {trip}");

        step.Order = nextStep.Order + 1;

        for (int i = nextStepIndex + 1; i < trip.Steps.Count; i++)
        {
            trip.Steps[i].Order++;
        }


        trip.Steps.Add(step);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task AddStepBeforAsync(Trip trip, Step previousStep, Step step)
    {
        trip.Steps = trip.Steps.OrderBy(step => step.Order).ToList();
        int previousStepIndex = trip.Steps.ToList().FindIndex(s => s.Order == previousStep.Order);

        if (previousStepIndex is -1)
            throw new ArgumentException($"L'étape suivante : {previousStep},  n'as pas été trouver dans le trip : {trip}");

        step.Order = previousStep.Order;

        for (int i = previousStepIndex; i < trip.Steps.Count; i++)
        {
            trip.Steps[i].Order++;
        }


        trip.Steps.Add(step);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task MoveStepToEndAsync(Trip trip, Step step)
    {
        trip.Steps = trip.Steps.OrderBy(step => step.Order).ToList();
        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int lastOrder = trip.Steps.Last().Order;

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        for (int i = stepIndex + 1; i < trip.Steps.Count; i++)
            trip.Steps[i].Order--;

        step.Order = lastOrder;

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task MoveStepBeforeAsync(Trip trip, Step step, Step previousStep)
    {
        trip.Steps = trip.Steps.OrderBy(step => step.Order).ToList();

        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int previousStepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == previousStep.StepId);

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        if (previousStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {previousStep},  n'as pas été trouver dans le trip : {trip}");

        step.Order = previousStep.Order;

        for (int i = previousStepIndex; i < stepIndex; i++)
            trip.Steps[i].Order++;

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task MoveStepAfterAsync(Trip trip, Step step, Step nextStep)
    {
        trip.Steps = trip.Steps.OrderBy(step => step.Order).ToList();

        int stepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == step.StepId);
        int nextStepIndex = trip.Steps.ToList().FindIndex(s => s.StepId == nextStep.StepId);

        if (stepIndex is -1)
            throw new ArgumentException($"L'étape : {step},  n'as pas été trouver dans le trip : {trip}");

        if (nextStepIndex is -1)
            throw new ArgumentException($"L'étape précédente : {nextStep},  n'as pas été trouver dans le trip : {trip}");

        step.Order = nextStep.Order;

        for (int i = stepIndex + 1; i <= nextStepIndex; i++)
            trip.Steps[i].Order--;

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task RemoveStepAsync(Step step)
    {
        _context.Step.Remove(step);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateStepAsync(Step step)
    {
        _context.Step.Update(step);
        await _context.SaveChangesAsync();
    }

    public async Task<Step?> GetStepByIdAsync(int stepId) => await _context.Step.Include(s => s.TravelBefore)
        .Include(s => s.Trip)
        .Include(s => s.TravelAfter)
        .Include(s => s.TravelBefore.TravelRoad)
        .Include(s => s.TravelAfter.TravelRoad)
        .FirstOrDefaultAsync(s => s.StepId == stepId);

    public async Task<Step?> GetByStepIdAsync(int stepId) => await _context.Step.FindAsync(stepId);
}
