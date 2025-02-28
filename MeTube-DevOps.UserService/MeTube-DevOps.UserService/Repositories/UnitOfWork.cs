using System;
using Microsoft.EntityFrameworkCore.Storage;
using MeTube_DevOps.UserService.Data;

namespace MeTube_DevOps.UserService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    private IDbContextTransaction _currentTransaction;
    public IUserRepository Users { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
        Users = new UserRepository(context);
    }
    public void Dispose()
    {
        context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _currentTransaction = await context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitAsync()
    {
        try
        {
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}