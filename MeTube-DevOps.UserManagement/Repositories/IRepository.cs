using System;

namespace MeTube_DevOps.UserManagement.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task AddAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}