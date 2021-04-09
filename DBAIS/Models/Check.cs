using System;
using System.Collections.Generic;

namespace DBAIS.Models
{
    public class Check
    {
        public string Number { get; set; } = null!;
        public string CardNum { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Vat { get; set; }
        public IList<Sale> Sales { get; set; } = ArraySegment<Sale>.Empty;
    }
}