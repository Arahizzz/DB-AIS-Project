using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class ProductsRepository
    {
        private readonly DbOptions _options;

        public ProductsRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<Product>> GetProducts()
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(@"select * from Product", conn);
            await conn.OpenAsync();
            await query.PrepareAsync();
            await using var reader = await query.ExecuteReaderAsync();

            var list = new List<Product>();
            while (reader.Read())
            {
                list.Add(Product.FromSql(reader));
            }
            
            return list;
        }

        public async Task AddProduct(Product product)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(
                @"insert into product (category_number, product_name, characteristics)
                         values (@category, @name, @characteristics)", conn
                );
            command.Parameters.Add(new NpgsqlParameter<int>("category", product.CategoryNum));
            command.Parameters.Add(new NpgsqlParameter<string>("name", product.Name));
            command.Parameters.Add(new NpgsqlParameter<string>("characteristics", product.Characteristics));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(
                @"update product
                         set category_number = @category, product_name = @name, characteristics = @characteristics
                         where product_id = @id)", conn
            );
            command.Parameters.Add(new NpgsqlParameter<int>("id", product.Id));
            command.Parameters.Add(new NpgsqlParameter<int>("category", product.CategoryNum));
            command.Parameters.Add(new NpgsqlParameter<string>("name", product.Name));
            command.Parameters.Add(new NpgsqlParameter<string>("characteristics", product.Characteristics));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteProduct(int id)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from product where category_number = @id
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<int>("id", id));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}