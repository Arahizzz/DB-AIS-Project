using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class EmployeeUserRepository
    {
        private readonly DbOptions _options;

        public EmployeeUserRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<EmployeeUser> GetEmployeeUserById(string id)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select id_employee, password, empl_surname, empl_name, empl_patronymic, role, salary, 
                                        date_of_birth, date_of_start, phone_number, city, street, zip_code
                                        from employee
                                        where id_employee = @id";
            await using var query = new NpgsqlCommand(queryString, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("id", id));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            if (!reader.Read())
                throw new EntityNotFoundException<EmployeeUser, string>(id);

            return GetEmployeeUserFromSql(reader);
        }

        public async Task<EmployeeUser> GetEmployeeUserByPhone(string phone)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select id_employee, password, empl_surname, empl_name, empl_patronymic, role, salary, 
                                        date_of_birth, date_of_start, phone_number, city, street, zip_code
                                        from employee
                                        where lower(phone_number) = @phone";
            await using var query = new NpgsqlCommand(queryString, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("phone", phone.ToLowerInvariant()));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            if (!reader.Read())
                throw new EntityNotFoundException<EmployeeUser, string>(phone);

            return GetEmployeeUserFromSql(reader);
        }
        
        public async Task<List<EmployeeUser>> GetEmployeeUsersByRole(string roleName)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select id_employee, password, empl_surname, empl_name, empl_patronymic, role, salary, 
                                        date_of_birth, date_of_start, phone_number, city, street, zip_code
                                        from employee
                                        where role = @role";
            await using var query = new NpgsqlCommand(queryString, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("role", roleName));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<EmployeeUser>();
            while (reader.Read())
            {
                list.Add(GetEmployeeUserFromSql(reader));
            }
            
            return list;
        }

        public async Task EditEmployeePassword(EmployeeUser employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
                                update employee set password = @password
                                where id_employee = @id
                        "
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
                new ("password", employee.Password)
            });

            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task EditEmployeePhone(EmployeeUser employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
                                update employee set phone_number = @phone
                                where id_employee = @id
                        "
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
                new ("phone", employee.PhoneNumber)
            });

            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
        
        public async Task EditEmployeeRole(EmployeeUser employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
                                update employee set role = @role
                                where id_employee = @id
                        "
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
                new ("role", employee.Role)
            });

            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }


        public async Task DeleteEmployee(string id)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
                                delete from employee where id_employee = @id
                        "
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("id", id));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task AddEmployee(EmployeeUser employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
                                        insert into employee (id_employee, password, empl_surname, empl_name, empl_patronymic, role, salary, date_of_birth, date_of_start, phone_number, city, street, zip_code)
                                        values (@id, @password, @surname, @name, @patronymic, @role, @salary, @date_of_birth, @date_of_start, @phone_number, @city, @street, @zip)
                                "
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
                new ("password", employee.Password),
                new ("surname", employee.Surname),
                new ("name", employee.Name),
                new ("patronymic", employee.Patronymic),
                new ("role", employee.Role),
                new ("salary", employee.Salary),
                new ("date_of_birth", employee.DateOfBirth),
                new ("date_of_start", employee.DateOfStart),
                new ("phone_number", employee.PhoneNumber),
                new ("city", employee.City),
                new ("street", employee.Street),
                new ("zip", employee.Zip)
            });

            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
        
        private static EmployeeUser GetEmployeeUserFromSql(IDataRecord reader)
        {
            return new EmployeeUser
            {
                Id = reader.GetString(0),
                Password = reader.GetString(1),
                Surname = reader.GetString(2),
                Name = reader.GetString(3),
                Patronymic = reader.GetString(4),
                Role = reader.GetString(5),
                Salary = reader.GetDecimal(6),
                DateOfBirth = reader.GetDateTime(7),
                DateOfStart = reader.GetDateTime(8),
                PhoneNumber = reader.GetString(9),
                City = reader.GetString(10),
                Street = reader.GetString(11),
                Zip = reader.GetString(12)
            };
        }
    }
}