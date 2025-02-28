using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeTube_DevOps.UserService.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

}