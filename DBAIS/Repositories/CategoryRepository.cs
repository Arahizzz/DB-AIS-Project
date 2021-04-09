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
    }
}