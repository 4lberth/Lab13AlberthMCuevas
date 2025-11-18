namespace Lab13AlberthMCuevas.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
}