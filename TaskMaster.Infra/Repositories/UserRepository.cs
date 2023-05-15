using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.Models;
using TaskMaster.Infra.Interfaces;
using TaskMaster.Infra.Interfaces.Repositories;

namespace TaskMaster.Infra.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
            tableName = "users";
            tableCols = new string[]
            {
                "id",
                "name",
                "email",
                "password",
                "refresh_token",
                "refresh_token_expires_at",
                "created_at",
                "updated_at"
            };
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var qry = $@"select
                            *
                        from {tableName}
                        where
                            email = @EMAIL";

            return await QueryFirstAsync<User>(qry, new { EMAIL = email });
        }

        public async Task<bool> UpdateRefreshToken(User user)
        {           
            var qry = $@"UPDATE {tableName} SET
                            refresh_token = @{nameof(user.RefreshToken)},
                            refresh_token_expires_at = @{nameof(user.RefreshTokenExpiresAt)}
                         WHERE
                            id = @{nameof(user.Id)};";

            return await ExecuteAsync(qry, user);
        }

        public async Task<User> GetUserById(int? id)
        {
            var qry = $@"SELECT 
                            *
                         FROM 
                            {tableName}
                         WHERE 
                            id = @ID";

            return await QueryFirstAsync<User>(qry, new
            {
                ID = id,
            });
        }

        public async Task<int> CreateUser(User data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            email,
                            password,
                            name
                         ) 
                        VALUES (
                            @{nameof(data.Email)},
                            @{nameof(data.Password)},
                            @{nameof(data.Name)}
                         );

                        SELECT SCOPE_IDENTITY() AS IdGerado;";

            return await QueryFirstAsync<int>(qry, data);
        }

        public async Task<bool> EmailExists(string email)
        {
            var qry = $@"SELECT 
                            email
                         FROM 
                            {tableName}
                         WHERE
                            email = @EMAIL";

            return (await QueryFirstAsync<User>(qry, new
            {
                EMAIL = email
            })) != null;
        }

        public async Task<IEnumerable<User>> ListUsers(UserFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var qry = $@"SELECT
                            {GetCols()}
                         FROM
                            {tableName}
                         {BuildFilter(filter)}
                         ORDER BY id
                         OFFSET ({offset.Item1} - 1) * {offset.Item2} ROWS 
                         FETCH NEXT {offset.Item2} ROWS ONLY;";

            return await QueryAsync<User>(qry, filter);
        }

        private string BuildFilter(UserFilter filter)
        {
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                filter.Search = filter.Search.ToUpper();

                result.Add($@"UPPER(email) LIKE '{filter.Search}%' 
                           OR UPPER(name) LIKE '{filter.Search}%'");
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

        public async Task<int> GetTotalItems(UserFilter filter)
        {
            var qry = $@"SELECT
                            count(*)
                         FROM
                            {tableName}
                         {BuildFilter(filter)};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<bool> UpdateUserPassword(int? id, string hashedPassword)
        {
            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            password = @PASSWORD
                         WHERE 
                            id = @ID;";

            return await ExecuteAsync(qry, new
            {
                PASSWORD = hashedPassword,
                ID = id,
            });
        }

        public async Task<bool> UpdateUser(int? id, User data)
        {
            if (id == null)
            {
                return false;
            }

            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                updatedCols.Add($"name = @{nameof(data.Name)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Email))
            {
                updatedCols.Add($"email = @{nameof(data.Email)}");
            }

            updatedCols.Add($"updated_at = current_timestamp");

            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            {string.Join(",", updatedCols)}
                         WHERE 
                            id = @{nameof(data.Id)};";

            data.Id = id.Value;

            return await ExecuteAsync(qry, data);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var qry = $@"DELETE FROM {tableName} WHERE id = @ID;";

            return await ExecuteAsync(qry, new
            {
                ID = id
            });
        }
    }
}
