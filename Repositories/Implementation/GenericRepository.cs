using Microsoft.EntityFrameworkCore;
using NCS.Prueba.Data;
using NCS.Prueba.Repositories.Interfaces;
using System.Linq.Expressions;

namespace NCS.Prueba.Repositories.Implementation;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> ReadOne(
        Expression<Func<TEntity, bool>> filtro,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        try
        {
            return await query.FirstOrDefaultAsync(filtro);
        }
        catch
        {
            throw;
        }
    }

    public Task<TEntity> Create(TEntity entidad)
    {
        try
        {
            _dbContext.Add(entidad);
            return Task.FromResult(entidad);
        }
        catch
        {
            throw;
        }
    }

    public Task<bool> Update(TEntity entidad)
    {
        try
        {
            _dbContext.Update(entidad);
            return Task.FromResult(true);
        }
        catch
        {
            throw;
        }
    }

    public Task<bool> Delete(TEntity entidad)
    {
        try
        {
            if (entidad is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.EstaEliminado = true;
                _dbContext.Update(entidad);
            }
            else
            {
                _dbContext.Remove(entidad);
            }
            return Task.FromResult(true);
        }
        catch
        {
            throw;
        }
    }

    public Task<IQueryable<TEntity>> ReadAll(
        Expression<Func<TEntity, bool>> filtro = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        try
        {
            var resultado = filtro == null ? query : query.Where(filtro);
            return Task.FromResult(resultado);
        }
        catch
        {
            throw;
        }
    }
}
