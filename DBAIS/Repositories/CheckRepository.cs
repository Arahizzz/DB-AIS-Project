using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Options;
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
",conn)
            {
                Transaction = transaction
            };
            
            checkCommand.Parameters.AddRange(new NpgsqlParameter[]
            {
                new ("check_num", check.Number),
                new ("id_empl", check.EmployeeId),
                new ("card_num", check.CardNum),
                new ("date", check.Date),
                new ("sum", check.Total),
                new ("vat", check.Vat)
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
                    new ("upc", sale.Upc),
                    new ("check", check.Number),
                    new ("count", sale.Count),
                    new ("price", sale.Price)
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
                new ("check_num", check.Number),
                new ("id_empl", check.EmployeeId),
                new ("card_num", check.CardNum),
                new ("date", check.Date),
                new ("sum", check.Total),
                new ("vat", check.Vat)
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
                    new ("upc", sale.Upc),
                    new ("check", check.Number),
                    new ("count", sale.Count),
                    new ("price", sale.Price)
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
            command.Parameters.Add(new NpgsqlParameter<string>("check_number", checkNum));
            command.Parameters.Add(new NpgsqlParameter<string>("upc", upc));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}