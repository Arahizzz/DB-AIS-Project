using System;

namespace DBAIS.Models
{
    public class Check
    {
        public int Number { get; set; }
        public int CardNum { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Vat { get; set; }
    }
}