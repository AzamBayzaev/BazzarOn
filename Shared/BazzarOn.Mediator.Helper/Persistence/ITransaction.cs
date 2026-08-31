namespace BazzarOn.Mediator.Helper.Persistence;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
   
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
