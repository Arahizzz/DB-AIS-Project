using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Models.DTOs;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class EmployeeRepository
    {
        private const string SELECT_EMPLOYEE_HISTORY = @"SELECT
                e.empl_surname,
                c.print_date,
                c.check_number,
                p.product_name,
                s.product_number,
                s.selling_price * s.product_number AS price_sum
               FROM employee E
               INNER JOIN ""Check"" c ON e.id_employee = c.id_employee
               INNER JOIN sale s ON c.check_number = s.check_number
               INNER JOIN store_product sp ON s.UPC = sp.UPC
               INNER JOIN product p ON sp.id_product = p.id_product
               WHERE e.id_employee = @id AND e.role = 'cashier'; ";

        private readonly DbOptions _options;

        public EmployeeRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<SellingInfo>> GetEmployeeChecks(string employeeId)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(SELECT_EMPLOYEE_HISTORY, conn);
            query.Parameters.Add(new NpgsqlParameter<string>("id", employeeId));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();

            var list = new List<SellingInfo>();
            if (!reader.Read())
                return list;

            while (true)
            {
                var products = new List<SellingInfo.ProductsSoldInfo>();
                var cashierSurename = reader.GetString(0);
                var printDate = reader.GetDateTime(1);
                var currCheckNumber = reader.GetString(2);
                var info = new SellingInfo
                {
                    CashierSurename = cashierSurename,
                    PrintDate = printDate,
                    CheckNumber = currCheckNumber
                };

                string lastCheckNumber = currCheckNumber;
                while (lastCheckNumber == currCheckNumber)
                {
                    products.Add(new ()
                    {
                        ProductName = reader.GetString(3),
                        Count = reader.GetInt32(4),
                        TotalPrice = reader.GetDecimal(5)
                    });
                    if (!reader.Read())
                    {
                        info.Products = products;
                        list.Add(info);
                        return list;
                    }
                    else lastCheckNumber = reader.GetString(2);
                }

                info.Products = products;
                list.Add(info);
            }
        }

        public async Task AddEmployee(Employee employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        insert into employee (id_employee, empl_surname, empl_name, empl_patronymic, role, salary, date_of_birth, date_of_start, phone_number, city, street, zip_code)
        values (@id, @surname, @name, @patronymic, @role, @salary, @date_of_birth, @date_of_start, @phone_number, @city, @street, @zip)
"
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
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

        public async Task EditEmployee(Employee employee)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        update employee set empl_surname = @surname, empl_name = @name, 
        empl_patronymic = @patronymic, role = @role, salary = @salary, date_of_birth = @date_of_birth, 
        date_of_start = @date_of_start, phone_number = @phone_number, city = @city, street = @street, zip_code = @zip
        where id_employee = @id
"
                , conn);

            command.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("id", employee.Id),
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

        public async Task<List<Employee>> GetCashiers(Sort surname)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select id_employee, empl_surname, empl_name, empl_patronymic, role, salary, 
                                date_of_birth, date_of_start, phone_number, city, street, zip_code
                                from employee";
            queryString += surname switch
            {
                Sort.Ascending => " order by empl_surname asc",
                Sort.Descending => " order by empl_surname desc",
                _ => ""
            };
            await using var query = new NpgsqlCommand(queryString, conn);
            await conn.OpenAsync();
            await query.PrepareAsync();
            
            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<Employee>();
            while (reader.Read())
            {
                list.Add(GetEmployeeFromSql(reader));
            }

            return list;
        }

        private static Employee GetEmployeeFromSql(IDataRecord reader)
        {
            return new Employee
            {
                Id = reader.GetString(0),
                Surname = reader.GetString(1),
                Name = reader.GetString(2),
                Patronymic = reader.GetString(3),
                Role = reader.GetString(4),
                Salary = reader.GetDecimal(5),
                DateOfBirth = reader.GetDateTime(6),
                DateOfStart = reader.GetDateTime(7),
                PhoneNumber = reader.GetString(8),
                City = reader.GetString(9),
                Street = reader.GetString(10),
                Zip = reader.GetString(11)
            };
        }
    }
}