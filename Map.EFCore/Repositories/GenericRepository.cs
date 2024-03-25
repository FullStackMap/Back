using Map.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Map.EFCore.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    #region Properties

    protected readonly MapContext _context;

    #endregion Properties

    #region Constructor

    public GenericRepository(MapContext context) => _context = context;

    #endregion Constructor

    #region PublicMethods

    /// <inheritdoc/>
    public void Add(T entity) => _context.Set<T>().Add(entity);

    /// <inheritdoc/>
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    /// <inheritdoc/>
    public void AddRange(IList<T> entities) => _context.Set<T>().AddRange(entities);

    /// <inheritdoc/>
    public async Task AddRangeAsync(IList<T> entities) => await _context.Set<T>().AddRangeAsync(entities);

    /// <inheritdoc/>
    public T? GetById(Guid id) => _context.Set<T>().Find(id);

    /// <inheritdoc/>
    public T? GetById(int id) => _context.Set<T>().Find(id);

    /// <inheritdoc/>
    public bool IsExist(Guid id) => _context.Set<T>().Find(id) is not null;

    /// <inheritdoc/>
    public async Task<bool> IsExistAsync(Guid id) => await _context.Set<T>().FindAsync(id) is not null;

    /// <inheritdoc/>
    public IQueryable<T> GetAll() => _context.Set<T>().AsQueryable();

    /// <inheritdoc/>
    public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

    /// <inheritdoc/>
    public IList<T> Find(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression).ToList();

    /// <inheritdoc/>
    public void Remove(T entity) => _context.Set<T>().Remove(entity);

    /// <inheritdoc/>
    public void RemoveRange(IList<T> entities) => _context.Set<T>().RemoveRange(entities);

    /// <inheritdoc/>
    public void RemoveAll() => _context.Set<T>().RemoveRange(_context.Set<T>());

    #endregion PublicMethods
}