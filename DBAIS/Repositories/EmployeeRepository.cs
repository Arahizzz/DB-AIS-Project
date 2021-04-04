using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
                    products.Add(new()
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
    }
}