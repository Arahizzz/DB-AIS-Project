using System;

namespace DBAIS.Models
{
    public class Card
    {
        public string Number { get; set; } = null!;
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Patronymic { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Zip { get; set; }
        public int Percent { get; set; }
    }
}