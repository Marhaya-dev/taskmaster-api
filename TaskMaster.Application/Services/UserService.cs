using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.Helpers;
using TaskMaster.Domain.Models;
using TaskMaster.Domain.ViewModels.v1;
using TaskMaster.Domain.ViewModels.v1.Users;
using TaskMaster.Infra.Interfaces.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserResponse> CreateUser(UserRequest data)
        {
            var user = _mapper.Map<User>(data);

            await ValidateUserAsync(user);

            var id = await _userRepository.CreateUser(user);

            user = await _userRepository.GetUserById(id);

            return _mapper.Map<UserResponse>(user);
        }

        private async Task ValidateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ValidationResultException("Usuário Inválido");
            }

            if (await _userRepository.EmailExists(user.Email))
            {
                throw new ValidationResultException($"O e-mail informado não está disponível.");
            }
        }

        public async Task<UserListResponse> ListUsers(UserFilter filter)
        {
            var users = await _userRepository.ListUsers(filter);

            return new UserListResponse()
            {
                Metadata = new ListMeta()
                {
                    TotalItems = await _userRepository.GetTotalItems(filter),
                    Count = users.Count(),
                    Page = filter.GetPage(),
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<UserResponse>>(users),
            };
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(int id, UserPutRequest data)
        {
            var newUser = _mapper.Map<User>(data);
            var oldUser = await _userRepository.GetUserById(id);

            if (!string.IsNullOrWhiteSpace(data.OldPassword) && !string.IsNullOrWhiteSpace(data.NewPassword))
            {
                if (PasswordHelper.VerifyHashedPassword(oldUser.Password, data.OldPassword))
                {
                    await _userRepository.UpdateUserPassword(id, newUser.Password);
                }
                else
                {
                    throw new ValidationResultException("A senha antiga está incorreta");
                }
            }

            await _userRepository.UpdateUser(id, newUser);

            newUser = await _userRepository.GetUserById(id);

            return _mapper.Map<UserResponse>(newUser);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }
    }
}
