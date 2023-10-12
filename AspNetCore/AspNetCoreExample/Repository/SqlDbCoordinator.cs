using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AspNetCoreExample.Repository
{
    public interface ISqlDbCoordinator
    {
        Task<SqlCommand> CreateCommandAsync(string commandText);
        Task<DbDataReader> ExecuteReaderAsync(SqlCommand cmd);
    }

    public class SqlDbCoordinator : ISqlDbCoordinator
    {
        private readonly Lazy<Task<SqlConnection>> connection;
        private SqlTransaction transaction;

        public SqlDbCoordinator(string connectionString)
        {
            connection = new Lazy<Task<SqlConnection>>(async () =>
            {
                var cn = new SqlConnection(connectionString);
                await cn.OpenAsync();
                return cn;
            });
        }

        public async Task<SqlCommand> CreateCommandAsync(string commandText)
        {
            var cmd = (await connection.Value).CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        public async Task<DbDataReader> ExecuteReaderAsync(SqlCommand cmd)
        {
            return await cmd.ExecuteReaderAsync();
        }
    }
}