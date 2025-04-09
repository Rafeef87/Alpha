using System.Diagnostics;
using System.Linq.Expressions;
using Data.Context;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
{
    Task<bool> AddAsync(TEntity entity);
    Task<IEnumerable<TSelect>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? storBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TModel> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<bool> UpdateAsync(TEntity entity);
 Task<bool> DeleteAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
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
    public virtual async Task<IEnumerable<TModel>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? storBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);
        if (storBy != null)
            query = orderByDescending
                ? query.OrderByDescending(storBy)
                : query.OrderBy(storBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return result;
    }
    public virtual async Task<IEnumerable<TSelect>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector ,bool orderByDescending = false, Expression<Func<TEntity, object>>? storBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);
        if (storBy != null)
            query = orderByDescending
                ? query.OrderByDescending(storBy)
                : query.OrderBy(storBy);

        var entities = await query.Select(selector).ToListAsync();
        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return result;
    }
    public virtual async Task<TModel> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);


        var entity = await query.FirstOrDefaultAsync(where);
        if (entity == null)
            throw new Exception("Entity not found");
        var result = entity.MapTo<TModel>();
        return result;
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