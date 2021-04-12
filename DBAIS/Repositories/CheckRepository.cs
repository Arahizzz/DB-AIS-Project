using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Models.DTOs;
using DBAIS.Options;
using DBAIS.Repositories.Utils;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class CheckRepository
    {
        private readonly DbOptions _options;

        public CheckRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task AddCheck(Check check)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();

            await using var checkCommand = new NpgsqlCommand(@"
            insert into ""Check"" (check_number, id_employee, card_number, print_date, sum_total, vat)
            values (@check_num, @id_empl, @card_num, @date, @sum, @vat)
", conn)
            {
                Transaction = transaction
            };

            checkCommand.Parameters.AddRange(new NpgsqlParameter[]
            {
                new NpgsqlParameter<string>("check_num", check.Number),
                new NpgsqlParameter<string>("id_empl", check.EmployeeId),
                new NpgsqlParameter<string?>("card_num", check.CardNum),
                new NpgsqlParameter<DateTime>("date", check.Date),
                new NpgsqlParameter<decimal>("sum", check.Total),
                new NpgsqlParameter<decimal>("vat", check.Vat)
            });

            await checkCommand.PrepareAsync();
            await checkCommand.ExecuteNonQueryAsync();

            foreach (var sale in check.Sales)
            {
                await using var salesCommand = new NpgsqlCommand(@"
                insert into sale (upc, check_number, product_number, selling_price)
                values (@upc, @check, @count, @price)
", conn)
                {
                    Transaction = transaction
                };

                salesCommand.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter<string>("upc", sale.Upc),
                    new NpgsqlParameter<string>("check", check.Number),
                    new NpgsqlParameter<int>("count", sale.Count),
                    new NpgsqlParameter<decimal>("price", sale.Price)
                });

                await salesCommand.PrepareAsync();
                await salesCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task UpdateCheck(Check check)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();

            await using var checkCommand = new NpgsqlCommand(@"
            update ""Check"" 
            set id_employee = @id_empl, 
                card_number = @card_num, print_date = @date, sum_total = @sum, vat = @vat
            where check_number = @check_num
", conn)
            {
                Transaction = transaction
            };

            checkCommand.Parameters.AddRange(new NpgsqlParameter[]
            {
                new NpgsqlParameter<string>("check_num", check.Number),
                new NpgsqlParameter<string>("id_empl", check.EmployeeId),
                new NpgsqlParameter<string?>("card_num", check.CardNum),
                new NpgsqlParameter<DateTime>("date", check.Date),
                new NpgsqlParameter<decimal>("sum", check.Total),
                new NpgsqlParameter<decimal>("vat", check.Vat)
            });

            await checkCommand.PrepareAsync();
            await checkCommand.ExecuteNonQueryAsync();

            foreach (var sale in check.Sales)
            {
                await using var salesCommand = new NpgsqlCommand(@"
                update sale
                set upc = @upc, product_number = @count, selling_price = @price
                where check_number = @check
", conn)
                {
                    Transaction = transaction
                };

                salesCommand.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter<string>("upc", sale.Upc),
                    new NpgsqlParameter<string>("check", check.Number),
                    new NpgsqlParameter<int>("count", sale.Count),
                    new NpgsqlParameter<decimal>("price", sale.Price)
                });

                await salesCommand.PrepareAsync();
                await salesCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task DeleteProductFromCheck(string checkNum, string upc)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from sale where check_number = @check and upc = @upc
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("check", checkNum));
            command.Parameters.Add(new NpgsqlParameter<string>("upc", upc));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteCheck(string checkNum)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from ""Check"" where check_number = @check
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("check", checkNum));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<PurchaseInfo>> GetCustomerChecks(string cardNum)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(
                @"SELECT c.check_number,
                           c.print_date,
                           c.sum_total,
                           sp.upc,
                           p.id_product,
                           p.product_name,
                           s.product_number,
                           s.selling_price
                    FROM ""Check"" c
                    inner join sale s on c.check_number = s.check_number
                    inner join store_product sp on s.upc = sp.upc
                    inner join product p on sp.id_product = p.id_product
                    where c.card_number = @cardNum
                ",
                conn
            );
            query.Parameters.Add(new NpgsqlParameter<string>("cardNum", cardNum));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();

            var list = new List<PurchaseInfo>();
            if (!reader.Read())
                return list;

            while (true)
            {
                var products = new List<PurchaseInfo.ProductInfo>();
                var currId = reader.GetString(0);
                var info = new PurchaseInfo
                {
                    CheckNumber = currId,
                    PrintDate = reader.GetDateTime(1),
                    TotalSum = reader.GetDecimal(2)
                };

                string lastId = currId;
                while (lastId == currId)
                {
                    products.Add(new ()
                    {
                        Upc = reader.GetString(3),
                        Id = reader.GetInt32(4),
                        Name = reader.GetString(5),
                        Count = reader.GetInt32(6),
                        Price = reader.GetDecimal(7)
                    });
                    if (!reader.Read())
                    {
                        info.Products = products;
                        list.Add(info);
                        return list;
                    }
                    else lastId = reader.GetString(0);
                }

                info.Products = products;
                list.Add(info);
            }
        }

        public async Task<List<Check>> GetChecks(string? cashier, DateTime? from, DateTime? to)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = new StringBuilder(
                @"select c.check_number, id_employee, card_number, print_date, sum_total, 
                                vat, upc, product_number, selling_price 
                                from ""Check"" c left join sale s on c.check_number = s.check_number");

            if (cashier != null || from != null || to != null)
                queryString.Append(" where");
            if (cashier != null)
                queryString.Append(" c.id_employee = @empl");
            if (from != null)
            {
                if (cashier != null)
                    queryString.Append(" and");
                queryString.Append(" c.print_date >= @from");
            }

            if (to != null)
            {
                if (cashier != null || from != null)
                    queryString.Append(" and");
                queryString.Append(" c.print_date <= @to");
            }


            await using var query = new NpgsqlCommand(queryString.ToString(), conn);
            if (cashier != null)
                query.Parameters.Add(new NpgsqlParameter<string>("empl", cashier));
            if (from != null)
                query.Parameters.Add(new NpgsqlParameter<DateTime>("from", from.Value));
            if (to != null)
                query.Parameters.Add(new NpgsqlParameter<DateTime>("to", to.Value));

            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            return GetChecksFromSql(reader);
        }

        public async Task<Check?> GetCheck(string number)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = new StringBuilder(
                @"select c.check_number, id_employee, card_number, print_date, sum_total, 
                                vat, upc, product_number, selling_price 
                                from ""Check"" c left join sale s on c.check_number = s.check_number
                                where c.check_number = @number");
            
            await using var query = new NpgsqlCommand(queryString.ToString(), conn);
            query.Parameters.Add(new NpgsqlParameter<string>("number", number));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            return GetChecksFromSql(reader).FirstOrDefault();
        }

        private static List<Check> GetChecksFromSql(IDataReader reader)
        {
            var list = new List<Check>();
            if (!reader.Read())
                return list;

            Check? check = null;
            while (reader.Read())
            {
                var currId = reader.GetString(0);
                if (check?.Number != currId)
                {
                    check = new Check
                    {
                        Number = currId,
                        EmployeeId = reader.GetString(1),
                        CardNum = reader.GetValueOrDefault<string>(2),
                        Date = reader.GetDateTime(3),
                        Total = reader.GetDecimal(4),
                        Vat = reader.GetDecimal(5),
                        Sales = new List<Sale>()
                    };
                    list.Add(check);
                }

                if (!reader.IsDBNull(6))
                {
                    check.Sales.Add(new ()
                    {
                        Upc = reader.GetString(6),
                        Check = currId,
                        Count = reader.GetInt32(7),
                        Price = reader.GetDecimal(8)
                    });
                }
            }

            return list;
        }

        public async Task<List<PromotionalCheck>> GetPromotionalChecks()
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(SQL.VadymQueries.VADYM_QUERY_4, conn);
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<PromotionalCheck>();
            while (reader.Read())
            {
                list.Add(new PromotionalCheck
                {
                    Number = reader.GetString(0)
                });
            }
            return list;
        }

        public async Task<int> GetProductCount(string upc, DateTime from, DateTime to)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(@"
            select sum(s.product_number)
            from sale s inner join ""Check"" C on C.check_number = sale.check_number
            where s.upc = @upc and C.print_date between @from and @to
", conn);
            query.Parameters.AddRange(new NpgsqlParameter[]
            {
                new NpgsqlParameter<string>("upc", upc),
                new NpgsqlParameter<DateTime>("from", from),
                new NpgsqlParameter<DateTime>("to", to)
            });
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            reader.Read();

            return reader.GetInt32(0);
        }
    }

}