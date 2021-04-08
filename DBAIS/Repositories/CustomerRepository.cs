using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class CustomerRepository
    {
        private readonly DbOptions _options;

        public CustomerRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
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
                    products.Add(new()
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
    }
}