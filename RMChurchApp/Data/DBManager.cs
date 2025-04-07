using Dapper;
using System.Data;
using MySqlConnector;

namespace RMChurchApp.Data
{
    public interface IDBManager
    {
        Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false);
        Task<T> QuerySingleAsync<T>(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false);
        Task<int> ExecuteAsync(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false);
        Task<int> ExecuteWithTransactionAsync(string connectionString, List<(string Query, object? Parameters, bool IsStoredProcedure)> commands);
        Task<long> ExecuteScalarAsync(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false);
    }


    public class DBManager : IDBManager
    {
        private MySqlConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false)
        {
            using var connection = CreateConnection(connectionString);
            return await connection.QueryAsync<T>(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
        }

        public async Task<T> QuerySingleAsync<T>(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false)
        {
            using var connection = CreateConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<T>(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
        }

        public async Task<int> ExecuteAsync(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false)
        {
            using var connection = CreateConnection(connectionString);
            return await connection.ExecuteAsync(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
        }

        public async Task<long> ExecuteScalarAsync(string connectionString, string query, object? parameters = null, bool isStoredProcedure = false)
        {
            using var connection = CreateConnection(connectionString);
            return await connection.ExecuteScalarAsync<long>(query, parameters, commandType: isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
        }

        public async Task<int> ExecuteWithTransactionAsync(string connectionString, List<(string Query, object? Parameters, bool IsStoredProcedure)> commands)
        {
            using var connection = CreateConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var cmd in commands)
                {
                    await connection.ExecuteAsync(cmd.Query, cmd.Parameters, transaction, commandType: cmd.IsStoredProcedure ? CommandType.StoredProcedure : CommandType.Text);
                }

                transaction.Commit();
                return 1; // success
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
