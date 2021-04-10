using System.Threading;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DBAIS
{
    public class EmployeeRoleStore : IRoleStore<string>
    {
        private readonly RoleRepository _roles;

        public EmployeeRoleStore(RoleRepository roles)
        {
            _roles = roles;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(string role, CancellationToken cancellationToken)
        {
            await _roles.AddRole(role);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> UpdateAsync(string role, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(string role, CancellationToken cancellationToken)
        {
            await _roles.DeleteRole(role);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(string role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role);
        }

        public Task<string> GetRoleNameAsync(string role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role);
        }

        public async Task SetRoleNameAsync(string role, string roleName, CancellationToken cancellationToken)
        {
            await _roles.UpdateRoleName(role, roleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(string role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.ToLowerInvariant());
        }

        public Task SetNormalizedRoleNameAsync(string role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<string> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roles.GetRole(roleId);
        }

        public async Task<string> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _roles.GetRoleNormalized(normalizedRoleName);
        }
    }
}