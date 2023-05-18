namespace Persistencia.Interfaces
{
    public interface IReadDbContext
    {
        Task<IEnumerable<T>> ExecuteSp<T>(string spName, CancellationToken cancellationToken);
        Task<IEnumerable<T>> ExecuteSp<T>(string spName, IEnumerable<ParameterStored> Parameter, CancellationToken cancellationToken);
        Task<T> ExecuteFirstOrDefaultSpAsync<T>(string spName, IEnumerable<ParameterStored> Parameter, CancellationToken cancellationToken);
    }
}
