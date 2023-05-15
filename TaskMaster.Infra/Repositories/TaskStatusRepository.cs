using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Infra.Interfaces;
using TaskMaster.Infra.Interfaces.Repositories;
using TaskStatus = TaskMaster.Domain.Models.TaskStatus;

namespace TaskMaster.Infra.Repositories
{
    public class TaskStatusRepository : Repository, ITaskStatusRepository
    {
        public TaskStatusRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            tableName = "task_status";
            tableCols = new string[]
            {
                "name",
                "description",
                "created_at",
                "updated_at",
            };
        }

        public async Task<int> CreateTaskStatus(TaskStatus data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name,
                            description
                        ) 
                        VALUES (
                            @{nameof(data.Name)},
                            @{nameof(data.Description)}
                         );

                        SELECT SCOPE_IDENTITY() AS IdGerado;";

            return await QueryFirstAsync<int>(qry, data);
        }

        public async Task<TaskStatus> GetTaskStatusById(int id)
        {
            var qry = $@"SELECT 
                            *
                         FROM
                            {tableName}
                         WHERE
                            id = @ID;";

            return await QueryFirstAsync<TaskStatus>(qry, new
            {
                ID = id
            });
        }

        public async Task<IEnumerable<TaskStatus>> ListTaskStatus(TaskStatusFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var qry = $@"SELECT
                            *
                         FROM
                            {tableName}
                        {BuildFilter(filter)}
                         ORDER BY id
                         OFFSET ({offset.Item1} - 1) * {offset.Item2} ROWS 
                         FETCH NEXT {offset.Item2} ROWS ONLY;";

            return await QueryAsync<TaskStatus>(qry, filter);
        }

        private string BuildFilter(TaskStatusFilter filter)
        {
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.ToUpper();

                result.Add($@"UPPER(name) LIKE '{filter.Search}%'");
            }

            return result.Any() ? $"WHERE {string.Join(" AND ", result)}" : null;
        }

        public async Task<int> GetTotalItems(TaskStatusFilter filter)
        {
            var qry = $@"SELECT 
                            count(*)
                         FROM
                            {tableName}
                        {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<bool> UpdateTaskStatus(int id, TaskStatus data)
        {
            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                updatedCols.Add($"name = @{nameof(data.Name)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Description))
            {
                updatedCols.Add($"description = @{nameof(data.Description)}");
            }

            updatedCols.Add($"updated_at = current_timestamp");

            var qry = $@"UPDATE {tableName} 
                         SET
                            {string.Join(",", updatedCols)}
                         WHERE
                            id = @{nameof(data.Id)};";

            data.Id = id;

            return await ExecuteAsync(qry, data);
        }

        public async Task<bool> DeleteTaskStatus(int id)
        {
            var qry = $@"DELETE FROM 
                            {tableName} 
                         WHERE 
                            id = @ID;";

            return await ExecuteAsync(qry, new
            {
                ID = id
            });
        }
    }
}
