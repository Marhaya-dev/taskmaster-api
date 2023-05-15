using Dapper;
using System.Data;
using System.Data.SqlClient;
using TaskMaster.Infra.Interfaces;

namespace TaskMaster.Infra
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new SqlConnection(_connectionString);
        }
    }
}
