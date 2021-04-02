using System.Data.Common;

namespace DBAIS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryNum { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Characteristics { get; set; } = string.Empty;

        public static Product FromSql(DbDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                CategoryNum = reader.GetInt32(1),
                Name = reader.GetString(2),
                Characteristics = reader.GetString(3)
            };
        }
    }
}