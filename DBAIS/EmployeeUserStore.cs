using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DBAIS.Models;
using Microsoft.AspNetCore.Identity;

namespace DBAIS.Repositories
{
    public class EmployeeUserStore : IUserRoleStore<EmployeeUser>, IUserPasswordStore<EmployeeUser>
    {
        private readonly EmployeeUserRepository _repository;

        public EmployeeUserStore(EmployeeUserRepository repository)
        {
            _repository = repository;
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public async Task SetUserNameAsync(EmployeeUser user, string userName, CancellationToken cancellationToken)
        {
            await _repository.EditEmployeePhone(user);
        }

        public Task<string> GetNormalizedUserNameAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber.ToLowerInvariant());
        }

        public Task SetNormalizedUserNameAsync(EmployeeUser user, string normalizedPhone, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            await _repository.AddEmployee(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            await _repository.EditEmployeePassword(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            await _repository.DeleteEmployee(user.Id);
            return IdentityResult.Success;
        }

        public async Task<EmployeeUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _repository.GetEmployeeUserById(userId);
        }

        public async Task<EmployeeUser> FindByNameAsync(string phone, CancellationToken cancellationToken)
        {
            return await _repository.GetEmployeeUserByPhone(phone);
        }

        public async Task AddToRoleAsync(EmployeeUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Role = roleName;
            await _repository.EditEmployeeRole(user);
        }

        public async Task RemoveFromRoleAsync(EmployeeUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Role = "";
            await _repository.EditEmployeeRole(user);
        }

        public Task<IList<string>> GetRolesAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            IList<string> roles = user.Role != "" ? new[] {user.Role} : ArraySegment<string>.Empty;
            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(EmployeeUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Role == roleName);
        }

        public async Task<IList<EmployeeUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _repository.GetEmployeeUsersByRole(roleName);
        }

        public Task SetPasswordHashAsync(EmployeeUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(EmployeeUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }
    }
}