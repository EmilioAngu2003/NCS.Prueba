using Microsoft.EntityFrameworkCore.Storage;
using NCS.Prueba.Data;
using NCS.Prueba.Repositories.Interfaces;

namespace NCS.Prueba.Repositories.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async Task<int> CommitChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        if (_transaction != null)
        {
            await action();
            await _context.SaveChangesAsync();
            return;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _transaction = transaction;
            await action();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _transaction = null;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
    {
        if (_transaction != null)
        {
            var result = await action();
            await _context.SaveChangesAsync();
            return result;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _transaction = transaction;
            var result = await action();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _transaction = null;
        }
    }
}
