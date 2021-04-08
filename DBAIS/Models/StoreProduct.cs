using System;

namespace DBAIS.Models
{
    public class StoreProduct
    {
        public string Upc { get; set; } = string.Empty;
        public string? UpcPromotional { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsPromotion { get; set; }
    }
}