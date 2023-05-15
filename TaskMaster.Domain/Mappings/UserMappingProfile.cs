using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TaskMaster.Domain.Helpers;
using TaskMaster.Domain.Models;
using TaskMaster.Domain.ViewModels.v1.Users;

namespace TaskMaster.Domain.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<UserRequest, User>()
                .ForMember(a => a.Password, b => b.MapFrom(c => PasswordHelper.GetHash(c.Password, KeyDerivationPrf.HMACSHA512)))
            ;

            CreateMap<UserPutRequest, User>()
                .ForMember(a => a.Password, b => b.MapFrom(c => PasswordHelper.GetHash(c.NewPassword, KeyDerivationPrf.HMACSHA512)))
            ;
        }
    }
}
