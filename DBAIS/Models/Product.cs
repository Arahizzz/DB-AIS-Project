using System.Data.Common;

namespace DBAIS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Characteristics { get; set; } = string.Empty;
        public CategoryModel Category { get; set; }

        public class CategoryModel
        {
            public int Number { get; set; }
            public string Name { get; set; } = string.Empty;
        }
        public static Product FromSql(DbDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Characteristics = reader.GetString(2),
                Category = new CategoryModel{ Number = reader.GetInt32(3), Name = reader.GetString(4) }
            };
        }
    }
}