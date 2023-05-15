using System;
using Dapper;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskMaster.Infra.Interfaces;

namespace TaskMaster.Infra.Repositories
{
    public abstract class Repository
    {
        private IDbConnectionFactory _connectionFactory;

        public Repository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected string tableName;
        protected IEnumerable<string> tableCols { private get;  set; }

        protected int MaxAttempts = 2;
        protected int AttemptDelay = 2000;

        protected delegate T QueryMethod<T>(string qry, object data);

        protected virtual string GetCols(string prefix = null, IEnumerable<string> except = null)
        {
            var cols = new List<string>();

            foreach (var col in tableCols)
            {
                if (!except?.Contains(col) ?? true)
                {
                    if (!string.IsNullOrWhiteSpace(prefix))
                    {
                        cols.Add($"{prefix}.{col}");
                    }
                    else
                    {
                        cols.Add(col);
                    }
                }
            }

            return string.Join(",", cols);
        }

        public (int, int) Paginate(int page, int limit = 30)
        {
            if (page < 1)
            {
                page = 1;
            }
            
            if (limit > 50)
            {
                limit = 50;
            }
            else if (limit < 1)
            {
                limit = 30;
            }

            int inicio = 1 + ((page - 1) * limit);
            int fim = inicio + limit - 1;

            return (inicio, fim);
        }
        
        protected virtual bool Execute(string qry, object data,int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    connection.Execute(qry);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    return Execute(qry, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<bool> ExecuteAsync(string qry, object data, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    await connection.ExecuteAsync(qry, data);
                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await ExecuteAsync(qry, data, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<bool> ExecuteAsync(string qry, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    await connection.ExecuteAsync(qry);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await ExecuteAsync(qry, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        } 

        protected virtual async Task<IEnumerable<T>> QueryAsync<T>(string qry, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    var result = await connection.QueryAsync<T>(qry);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryAsync<T>(qry, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<IEnumerable<T>> QueryAsync<T>(string qry, object data, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    var result = await connection.QueryAsync<T>(qry, data);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryAsync<T>(qry, data, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<T> QueryFirstAsync<T>(string qry, object data, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    var result = await connection.QueryFirstOrDefaultAsync<T>(qry, data);

                    return result;
                }

            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryFirstAsync<T>(qry, data, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<T> QueryFirstAsync<T>(string qry, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    var result = await connection.QueryFirstOrDefaultAsync<T>(qry);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryFirstAsync<T>(qry, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual T QueryFirst<T>(string qry, object data, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    var result = connection.QueryFirstOrDefault<T>(qry, data);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    return QueryFirst<T>(qry, data, attempts);
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
