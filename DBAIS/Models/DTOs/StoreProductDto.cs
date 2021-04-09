namespace DBAIS.Models.DTOs
{
    public class StoreProductDto
    {
        public string Upc { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; } 
        public int Count { get; init; } 
        public bool IsPromotion { get; init; }
    };
}