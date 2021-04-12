using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DBAIS.Repositories
{
    public class CategoryRepository
    {
        private readonly DbOptions _options;

        private static readonly string VADYM_QUERY_2 = @"
SELECT
res.year,
TO_CHAR(
	TO_DATE (res.month::text, 'MM'), 'Month'
) AS month,
res.category_name,
res.quantity as quantity
FROM (
	(
	SELECT grouped_by_month.year, grouped_by_month.month, grouped_by_month.category_number, SUM(grouped_by_month.product_number) AS quantity
	FROM (
		SELECT EXTRACT(YEAR FROM print_date) AS year,EXTRACT(MONTH FROM print_date) AS month, p.category_number, s.product_number
			FROM ""Check"" c
			INNER JOIN sale s ON c.check_number=s.check_number
            INNER JOIN store_product sp ON s.UPC = sp.UPC

            INNER JOIN product p ON sp.id_product = p.id_product
        ) grouped_by_month
    GROUP BY grouped_by_month.year, grouped_by_month.month, grouped_by_month.category_number
) grouped_by_category
INNER JOIN category ca ON grouped_by_category.category_number = ca.category_number) res
ORDER BY year, month, quantity DESC;
            ";

        public CategoryRepository(IOptions<DbOptions> options)
        {
            _options = options.Value;
        }

        public async Task AddCategory(Category category)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        insert into category (category_name) values (@name)
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("name", category.Name));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        update category 
        set category_name = @name
        where category_number = @id
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<int>("id", category.Number));
            command.Parameters.Add(new NpgsqlParameter<string>("name", category.Name));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteCategory(int id)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from category where category_number = @id
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<int>("id", id));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Category>> GetCategoriesAlphabetical()
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(@"
        select category_number, category_name from category order by category_name
"
                , conn);
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<Category>();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    Number = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return list;
        }

        public async Task<List<BestCategory>> GetBestCategories()
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var query = new NpgsqlCommand(VADYM_QUERY_2, conn);
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<BestCategory>();
            while (reader.Read())
            {
                list.Add(new BestCategory
                {
                    Year = Convert.ToInt32(reader.GetDouble(0)),
                    Month = reader.GetString(1),
                    Name = reader.GetString(2),
                    Quantity = reader.GetInt32(3),
                });
            }
            return list;
        }

    }
}