using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
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

        public async Task AddCard(Card card)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        insert into customer_card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street, zip_code, percent) 
        values (@card_number, @surname, @name, @patronymic, @phone_number, @city, @street, @zip, @percent)
"
                , conn);
            command.Parameters.AddRange(new NpgsqlParameter []
            {
                new ("card_number", card.Number),
                new ("surname", card.Surname),
                new ("name", card.Name),
                new ("patronymic", card.Patronymic),
                new ("phone_number", card.Phone),
                new ("city", card.City),
                new ("street", card.Street),
                new ("zip", card.Zip),
                new ("percent", card.Percent)
            });
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
        
        public async Task EditCard(Card card)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        update customer_card 
        set cust_surname = @surname, cust_name = @name, 
            cust_patronymic = @patronymic, phone_number = @phone_number, city = @city,
            street = @street, zip_code = @zip, percent = @percent
        where card_number = @card_number
"
                , conn);
            command.Parameters.AddRange(new NpgsqlParameter []
            {
                new ("card_number", card.Number),
                new ("surname", card.Surname),
                new ("name", card.Name),
                new ("patronymic", card.Patronymic),
                new ("phone_number", card.Phone),
                new ("city", card.City),
                new ("street", card.Street),
                new ("zip", card.Zip),
                new ("percent", card.Percent)
            });
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
        
        public async Task DeleteCustomer(string number)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            await using var command = new NpgsqlCommand(@"
        delete from category where category_number = @num
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("num", number));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}