namespace NCS.Prueba.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitChangesAsync();

    Task ExecuteInTransactionAsync(Func<Task> action);

    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
}
