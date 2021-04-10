using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DBAIS.Models;
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
        delete from customer_card where card_number = @num
"
                , conn);
            command.Parameters.Add(new NpgsqlParameter<string>("num", number));
            await conn.OpenAsync();
            await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Card>> GetCards(int? percent)
        {
            await using var conn = new NpgsqlConnection(_options.ConnectionString);
            var queryString = @"select card_number, cust_surname, cust_name, cust_patronymic, phone_number, 
                                city, street, zip_code, percent from customer_card";
            if (percent != null)
                queryString += " where percent = @percent";
            await using var query = new NpgsqlCommand(queryString, conn);
            if (percent != null)
                query.Parameters.Add(new NpgsqlParameter<int>("percent", percent.Value));
            await conn.OpenAsync();
            await query.PrepareAsync();

            await using var reader = await query.ExecuteReaderAsync();
            var list = new List<Card>();
            while (reader.Read())
            {
                list.Add(GetCardFromSql(reader));
            }
            
            return list;
        }

        private static Card GetCardFromSql(IDataRecord reader)
        {
            return new Card
            {
                Number = reader.GetString(0),
                Surname = reader.GetString(1),
                Name = reader.GetString(2),
                Patronymic = reader.GetString(3),
                Phone = reader.GetString(4),
                City = reader.GetString(5),
                Street = reader.GetString(6),
                Zip = reader.GetString(7),
                Percent = reader.GetInt32(8)
            };
        }
    }
}