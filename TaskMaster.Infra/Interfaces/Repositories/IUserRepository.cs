using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.Models;

namespace TaskMaster.Infra.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdateRefreshToken(User user);
        Task<User> GetUserById(int? id);
        Task<int> CreateUser(User data);
        Task<bool> EmailExists(string email);
        Task<IEnumerable<User>> ListUsers(UserFilter filter);
        Task<int> GetTotalItems(UserFilter filter);
        Task<bool> UpdateUserPassword(int? id, string hashedPassword);
        Task<bool> UpdateUser(int? id, User data);
        Task<bool> DeleteUser(int id);
    }
}
