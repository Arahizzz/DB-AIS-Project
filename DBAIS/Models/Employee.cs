using System;

namespace DBAIS.Models
{
    public class Employee
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfStart { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
}