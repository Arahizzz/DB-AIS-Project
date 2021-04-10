using System.Threading.Tasks;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class RoleRepository
    {
        private readonly DbOptions _options;

        public RoleRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> GetRole(string name)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select role_name from role where role_name = @name";
            await using var query = new NpgsqlCommand(queryString, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("name",name));
            await conn.OpenAsync();
            await query.PrepareAsync();
            
            await using var reader = await query.ExecuteReaderAsync();
            if (!reader.Read())
                throw new EntityNotFoundException<string, string>(name);

            return reader.GetString(0);
        }
        
        public async Task<string> GetRoleNormalized(string normalized)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select role_name from role where lower(role_name) = @name";
            await using var query = new NpgsqlCommand(queryString, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("name",normalized));
            await conn.OpenAsync();
            await query.PrepareAsync();
            
            await using var reader = await query.ExecuteReaderAsync();
            if (!reader.Read())
                throw new EntityNotFoundException<string, string>(normalized);

            return reader.GetString(0);
        }
        
        public async Task AddRole(string name)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var cmdString = @"insert into role (role_name) values (@name)";
            await using var cmd = new NpgsqlCommand(cmdString, conn);
            cmd.Parameters.Add(new NpgsqlParameter<string>("name",name));
            await conn.OpenAsync();
            await cmd.PrepareAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        
        public async Task DeleteRole(string name)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var cmdString = @"delete from role where role_name = @name";
            await using var cmd = new NpgsqlCommand(cmdString, conn);
            cmd.Parameters.Add(new NpgsqlParameter<string>("name",name));
            await conn.OpenAsync();
            await cmd.PrepareAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        
        public async Task UpdateRoleName(string oldName, string newName)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var cmdString = @"update role set role_name = @newName where role_name = @oldName";
            await using var cmd = new NpgsqlCommand(cmdString, conn);
            cmd.Parameters.Add(new NpgsqlParameter<string>("oldName",oldName));
            cmd.Parameters.Add(new NpgsqlParameter<string>("newName",newName));
            await conn.OpenAsync();
            await cmd.PrepareAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}