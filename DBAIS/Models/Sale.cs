namespace DBAIS.Models
{
    public class Sale
    {
        public string Upc { get; set; } = string.Empty;
        public string Check { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}