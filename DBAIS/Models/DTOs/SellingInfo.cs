using System;
using System.Collections.Generic;

namespace DBAIS.Models.DTOs
{
    public class SellingInfo
    {
        public string CashierSurename { get; set; } = String.Empty;
        public DateTime PrintDate { get; set; }
        public string CheckNumber { get; set; } = String.Empty;
        public IList<ProductsSoldInfo> Products { get; set; } = ArraySegment<ProductsSoldInfo>.Empty;

        public class ProductsSoldInfo
        {
            public string ProductName { get; set; } = String.Empty;
            public int Count { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }

}