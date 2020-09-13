﻿using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure
{
    public interface IUserService
    {
        Task<string> CreateAsync(CreateUserDto dto);
        Task UpdateAsync(UpdateUserDto dto);
        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}