namespace Map.EFCore.Interfaces;

public interface IUnitOfWork : IDisposable
{
    #region Properties

    ITripRepository Trip { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Completes the.
    /// </summary>
    /// <returns>An int.</returns>
    int Complete();

    /// <summary>
    /// Completes the async.
    /// </summary>
    /// <returns>A Task.</returns>
    Task<int> CompleteAsync();

    #endregion Methods
}