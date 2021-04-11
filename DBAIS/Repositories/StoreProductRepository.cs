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

        public async Task<StoreProduct> GetProduct(string upc)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(
                @"select upc, upc_prom, id_product, selling_price, products_number, promotional_product
                           from store_product where upc = @upc"
                , conn);
            query.Parameters.Add(new NpgsqlParameter<string>("upc", upc));
            await conn.OpenAsync();
            await query.PrepareAsync();
            await using var reader = await query.ExecuteReaderAsync();

            if (!reader.Read())
                throw new EntityNotFoundException<StoreProduct, string>(upc);

            return GetStoreProductFromSql(reader);
        }

        public async Task<List<StoreProductDto>> GetStoreProductsInfo(Sort name, Sort count, bool? promotionFilter)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select s.upc, p.product_name, s.selling_price, s.products_number, s.promotional_product
                           from store_product s inner join product p on p.id_product = s.id_product ";
            if (promotionFilter != null)
                queryString += " where s.promotional_product = @promo ";
            queryString += (name, count) switch
            {
                (Sort.Ascending, _) => "order by p.product_name asc",
                (Sort.Descending, _) => "order by p.product_name desc",
                (_, Sort.Ascending) => "order by s.products_number asc",
                (_, Sort.Descending) => "order by s.products_number desc",
                _ => ""
            };
            await using var query = new NpgsqlCommand(queryString, conn);
            if (promotionFilter != null)
                query.Parameters.Add(new NpgsqlParameter<bool>("promo", promotionFilter.Value));
            await conn.OpenAsync();
            await query.PrepareAsync();
            
            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<StoreProductDto>();
            while (reader.Read())
            {
                list.Add(GetStoreProductDtoFromSql(reader));
            }

            return list;
        }

        private static StoreProductDto GetStoreProductDtoFromSql(IDataRecord reader)
        {
            return new StoreProductDto
            {
                Upc = reader.GetString(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Count = reader.GetInt32(3),
                IsPromotion = reader.GetBoolean(4)
            };
        }

        private static StoreProduct GetStoreProductFromSql(IDataRecord reader)
        {
            return new StoreProduct
            {
                Upc = reader.GetString(0),
                UpcPromotional = reader.GetString(1),
                ProductId = reader.GetInt32(2),
                Price = reader.GetDecimal(3),
                Count = reader.GetInt32(4),
                IsPromotion = reader.GetBoolean(5)
            };
        }
    }
}