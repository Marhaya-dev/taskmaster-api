using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.Users;

namespace TaskMaster.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserRequest data);
        Task<UserListResponse> ListUsers(UserFilter filter);
        Task<UserResponse> GetUserById(int id);
        Task<UserResponse> UpdateUser(int id, UserPutRequest data);
        Task<bool> DeleteUser(int id);
    }
}
