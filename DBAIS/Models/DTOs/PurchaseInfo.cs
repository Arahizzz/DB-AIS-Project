using System;
using System.Collections.Generic;

namespace DBAIS.Models.DTOs
{
    public class PurchaseInfo
    {
        public string CheckNumber { get; set; } = String.Empty;
        public DateTime PrintDate { get; set; }
        public decimal TotalSum { get; set; }
        public IList<ProductInfo> Products { get; set; } = ArraySegment<ProductInfo>.Empty;

        public class ProductInfo
        {
            public string Upc { get; set; } = String.Empty;
            public int Id { get; set; }
            public string Name { get; set; } = String.Empty;
            public int Count { get; set; }
            public decimal Price { get; set; }
        }
    }
}