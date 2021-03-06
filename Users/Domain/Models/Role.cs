﻿using Shared;
using Shared.Entity;
using Shared.Exceptions;
using System;
using System.Runtime.CompilerServices;
using Users.Domain.Dtos;

[assembly: InternalsVisibleTo("Users.Domain.Tests")]
namespace Users.Domain.Models
{
    public class Role : Aggregate
    {
        public string Name { get; private set; }

        internal void UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ValidationException(ErrorMessages.RoleNameRequired);
            }

            if (name.Length > 45)
            {
                throw new ValidationException(ErrorMessages.RoleNameTooLong);
            }

            Name = name;
        }

        public void Update(UpdateRoleDto dto)
        {
            UpdateName(dto.Name);
        }

        public static Role Create(CreateRoleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var role = new Role();
            role.UpdateName(dto.Name);
            return role;
        }
    }
}
