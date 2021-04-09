using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class StoreProductRepository
    {
        private readonly DbOptions _options;

        public StoreProductRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task AddProduct(StoreProduct product)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(
                @"insert into store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
                         values (@upc, @upc_prom, @id_product, @selling_price, @products_number, @promotional_product)", conn
                );
            command.Parameters.Add(new NpgsqlParameter<string>("upc", product.Upc));
            command.Parameters.Add(new NpgsqlParameter<string?>("upc_prom", product.UpcPromotional));
            command.Parameters.Add(new NpgsqlParameter<int>("id_product", product.ProductId));
            command.Parameters.Add(new NpgsqlParameter<decimal>("selling_price", product.Price));
            command.Parameters.Add(new NpgsqlParameter<int>("products_number", product.Count));
            command.Parameters.Add(new NpgsqlParameter<bool>("promotional_product", product.IsPromotion));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProduct(StoreProduct product)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(
                @"update store_product
                         set upc_prom = @upc_prom, id_product = @id_product, 
                             selling_price = @selling_price, products_number = @products_number, promotional_product = @promotional_product
                         where upc = @upc", conn
            );
            command.Parameters.Add(new NpgsqlParameter<string>("upc", product.Upc));
            command.Parameters.Add(new NpgsqlParameter<string?>("upc_prom", product.UpcPromotional));
            command.Parameters.Add(new NpgsqlParameter<int>("id_product", product.ProductId));
            command.Parameters.Add(new NpgsqlParameter<decimal>("selling_price", product.Price));
            command.Parameters.Add(new NpgsqlParameter<int>("products_number", product.Count));
            command.Parameters.Add(new NpgsqlParameter<bool>("promotional_product", product.IsPromotion));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteProduct(string upc)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from store_product where upc = @upc
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("upc", upc));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}