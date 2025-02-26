using System;
using Microsoft.EntityFrameworkCore;

namespace MeTube_DevOps.UserManagement.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;

    public Repository(DbContext context)
    {
        Context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task RemoveAsync(TEntity entity)
    {
        await Task.Run(() => Context.Set<TEntity>().Remove(entity));
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        Context.SaveChanges();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }
}