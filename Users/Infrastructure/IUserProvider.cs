﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Domain.Dtos;

namespace Users.Infrastructure
{
    public interface IUserProvider
    {
        Task<UserDto> GetAsync(string id);
        Task<IReadOnlyList<UserDto>> GetListAsync(string searchTerm = null, string roleId = null);
    }
}