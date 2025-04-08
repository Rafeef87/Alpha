using System.Diagnostics;
using System.Linq.Expressions;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<bool> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy);
    Task<bool> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{

    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    #region CRUD
    //CREATE
    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
    //READ
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _table.ToListAsync();
        return entities;
    }
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var entity = await _table.FirstOrDefaultAsync(findBy);
        return entity ?? null!;
    }
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return exists;
    }

    //UPDATE
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
    //DELETE
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    #endregion
}
