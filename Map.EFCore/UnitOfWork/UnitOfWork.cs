using Map.EFCore.Interfaces;
using Map.EFCore.Repositories;

namespace Map.EFCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    #region Properties

    private readonly MapContext _context;

    public ITripRepository Trip { get; }

    #endregion Properties

    #region Constructor

    public UnitOfWork(MapContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Trip = new TripRepository(_context);
    }

    #endregion Constructor

    #region PublicMethods

    /// <inheritdoc/>
    public int Complete() => _context.SaveChanges();

    /// <inheritdoc/>
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    /// <inheritdoc/>
    public void Dispose() => _context.Dispose();

    /// <inheritdoc/>
    public async Task DisposeAsync() => await _context.DisposeAsync();

    #endregion PublicMethods
}