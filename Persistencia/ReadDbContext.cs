using Dapper;
using Microsoft.Extensions.Configuration;
using Persistencia.Interfaces;
using System.Data;

namespace Persistencia
{
    public class ReadDbContext : IReadDbContext
    {
        private readonly IDbConnection _dbConnection;
        public ReadDbContext(IDbConnection dbConnection, IConfiguration config)
        {
            _dbConnection = dbConnection;
            _dbConnection.ConnectionString = config["ConnectionStrings:AppConexion"];

        }

        public async Task<T> ExecuteFirstOrDefaultSpAsync<T>(string spName, IEnumerable<ParameterStored> Parameter, CancellationToken cancellationToken)
        {

            var valueParameter = new DynamicParameters();

            foreach (ParameterStored item in Parameter)
            {
                valueParameter.Add(item.ParameterName, item.ParameterValue, item.Type, item.Direction);
            }

            var result = await _dbConnection.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(spName
                , parameters: valueParameter
                , commandTimeout: 0
                , cancellationToken: cancellationToken
                , commandType: CommandType.StoredProcedure));

            return result;
        }

        public async Task<IEnumerable<T>> ExecuteSp<T>(string spName, IEnumerable<ParameterStored> Parameter, CancellationToken cancellationToken)
        {

            var valueParameter = new DynamicParameters();

            foreach (ParameterStored item in Parameter)
            {
                valueParameter.Add(item.ParameterName, item.ParameterValue, item.Type, item.Direction);
            }


            var result = await _dbConnection.QueryAsync<T>(
                new CommandDefinition(spName
                    , parameters: valueParameter
                    , cancellationToken: cancellationToken
                    , commandType: CommandType.StoredProcedure));
            return result;
        }

        public async Task<IEnumerable<T>> ExecuteSp<T>(string spName, CancellationToken cancellationToken)
        {

            var result = await _dbConnection.QueryAsync<T>(
                new CommandDefinition(spName
                    , cancellationToken: cancellationToken
                    , commandType: CommandType.StoredProcedure));
            return result;
        }
    }
}
