using System.Data;

namespace TaskMaster.Infra.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
        void SetConnectionString(string connectionString);
    }
}
