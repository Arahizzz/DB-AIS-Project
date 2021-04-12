namespace DBAIS.Models.DTOs
{
    public class CashierInteractionInfo
    {
        public string Customer { get; set; } = string.Empty;
        public string? EmployeeId { get; set; }
        public string? EmplName { get; set; }
        public string? EmplSurname { get; set; }
        public int Count { get; set; }
    }
}