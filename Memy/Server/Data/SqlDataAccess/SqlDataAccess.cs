using Dapper;

using System.Data;
using System.Data.SqlClient;

namespace Memy.Server.Data.SqlDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        public string ConnectionStringName { get; set; } = "Default";
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public string GetConnectionString()
        {
            return _config.GetConnectionString(ConnectionStringName);
        }

        public async Task<T> LoadData<T>(string sql)
        {
            try
            {
                string connectionString = GetConnectionString();

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    var data = await connection.QuerySingleAsync<T>(sql);
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IList<T>> LoadDataList<T>(string sql)
        {
            try
            {
                string connectionString = GetConnectionString();

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    var data = await connection.QueryAsync<T>(sql);
                    return data.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task SaveData<T>(string sql, T parameters)
        {
            try
            {
                string connectionString = GetConnectionString();

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
