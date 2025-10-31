using System.Linq.Expressions;

namespace NCS.Prueba.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> ReadOne(
        Expression<Func<TEntity, bool>> filtro,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> Create(TEntity entidad);

    Task<bool> Update(TEntity entidad);

    Task<bool> Delete(TEntity entidad);

    Task<IQueryable<TEntity>> ReadAll(
        Expression<Func<TEntity, bool>> filtro = null,
        params Expression<Func<TEntity, object>>[] includes);
}
