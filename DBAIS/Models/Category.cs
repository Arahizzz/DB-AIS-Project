using System;
using System.ComponentModel.DataAnnotations;

namespace DBAIS.Models
{
    public class Category
    {
        public int Number { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class BestCategory
    {
        public int Year { get; set; }

        public string Month { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}