using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Infra.Interfaces;
using TaskMaster.Infra.Interfaces.Repositories;
using Task = TaskMaster.Domain.Models.Task;

namespace TaskMaster.Infra.Repositories
{
    public class TaskRepository : Repository, ITaskRepository
    {
        public TaskRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            tableName = "tasks";
            tableCols = new string[]
            {
                "name",
                "description",
                "user_id",
                "status_id",
                "deadline",
                "created_at",
                "updated_at",
            };
        }

        public async Task<int> CreateTask(Task data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name,
                            description,
                            user_id,
                            status_id,
                            deadline
                         ) VALUES (
                            @{nameof(data.Name)},
                            @{nameof(data.Description)},
                            @{nameof(data.UserId)}, 
                            @{nameof(data.StatusId)},
                            @{nameof(data.Deadline)}
                         );

                        SELECT SCOPE_IDENTITY() AS IdGerado;";

            return await QueryFirstAsync<int>(qry, data);
        }

        public async Task<Task> GetTaskById(int id)
        {
            var qry = $@"SELECT 
                            *
                         FROM
                            {tableName}
                         WHERE
                            id = @ID;";

            return await QueryFirstAsync<Task>(qry, new
            {
                ID = id
            });
        }

        public async Task<IEnumerable<Task>> ListTask(TaskFilter filter)
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

            return await QueryAsync<Task>(qry, filter);
        }

        private string BuildFilter(TaskFilter filter)
        {
            var result = new List<string>();

            if (filter.UserId.HasValue)
            {
                result.Add($@"user_id = @{nameof(filter.UserId)}");
            }

            if (filter.StatusId.HasValue)
            {
                result.Add($@"status_id = @{nameof(filter.StatusId)}");
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.ToUpper();

                result.Add($@"UPPER(name) LIKE '{filter.Search}%'");
            }

            if (filter.CreatedAfter.HasValue)
            {
                result.Add($@"created_at > @{nameof(filter.CreatedAfter)}");
            }

            if (filter.UpdatedAfter.HasValue)
            {
                result.Add($@"updated_at > @{nameof(filter.UpdatedAfter)}");
            }

            return result.Any() ? $"WHERE {string.Join(" AND ", result)}" : null;
        }

        public async Task<int> GetTotalItems(TaskFilter filter)
        {
            var qry = $@"SELECT 
                            count(*)
                         FROM
                            {tableName}
                        {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<bool> UpdateTask(int id, Task data)
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

            if (data.UserId.HasValue)
            {
                updatedCols.Add($"user_id = @{nameof(data.UserId)}");
            }

            if (data.StatusId != 0)
            {
                updatedCols.Add($"status_id = @{nameof(data.StatusId)}");
            }

            if (data.Deadline != DateTime.MinValue)
            {
                updatedCols.Add($"deadline = @{nameof(data.Deadline)}");
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

        public async Task<bool> DeleteTask(int id)
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
