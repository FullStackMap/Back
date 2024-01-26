using Map.EFCore.Interfaces;

namespace Map.EFCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    #region Properties

    private readonly MapContext _context;
    //public IKeyRepository Keys { get; private set; }
    //public ITranslationRepository Translations { get; private set; }

    #endregion Properties

    #region Constructor

    public UnitOfWork(MapContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        //Keys = keys ?? throw new ArgumentNullException(nameof(keys));
        //Translations = translations ?? throw new ArgumentNullException(nameof(translations));
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