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
        private const string GetProductsJoinCategory = @"
                select p.id_product, p.product_name, p.characteristics, c.category_number, c.category_name
                from Product p INNER JOIN Category c ON p.category_number=c.category_number
            ";
        private readonly DbOptions _options;

        public ProductsRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<Product>> GetProducts()
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(GetProductsJoinCategory, conn);
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
            command.Parameters.Add(new NpgsqlParameter<int>("category", product.Category.Number));
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
                         where id_product = @id", conn
            );
            command.Parameters.Add(new NpgsqlParameter<int>("id", product.Id));
            command.Parameters.Add(new NpgsqlParameter<int>("category", product.Category.Number));
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
        delete from product where id_product = @id
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<int>("id", id));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Product>> GetProducts(string? category, Sort productName)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var sort = productName switch
            {
                Sort.Ascending => "order by product_name asc",
                Sort.Descending => "order by product_name desc",
                _ => ""
            };
            var queryString = category switch
            {
                null => GetProductsJoinCategory + sort,
                _ => GetProductsJoinCategory + " where c.category_name = @cat " + sort
            };
            
            await using var query = new NpgsqlCommand(queryString, conn);
            if (category != null)
                query.Parameters.Add(new NpgsqlParameter<string>("cat", category));
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
    }
}