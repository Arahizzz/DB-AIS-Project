using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.StaffPages
{
    [Authorize(Roles = "manager")]
    public class UpdateEmployeeModel : PageModel
    {

        // Current employee
        public Models.Employee Employee { get; set; }

        // Form data

        [BindProperty]
        [MaxLength(50)]
        public string Surname { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Name { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Patronym { get; set; }

        [BindProperty]
        [MaxLength(13)]
        public string Telephone { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string? City { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string? Street { get; set; }

        [BindProperty]
        [MaxLength(9)]
        public string? ZipCode { get; set; }

        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        [Range(0,double.MaxValue)]
        public decimal Salary { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateOfStart { get; set; }

        private readonly EmployeeRepository _employeeRepository;

        public UpdateEmployeeModel(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        private async Task InitModel(string number)
        {
            var cards = await _employeeRepository.GetCashiers(Models.Sort.None);
            Employee = cards.Find(x => x.Id.Equals(number));
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id);
            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                await InitModel(id);
                return Page();
            }
            else
            {
                var newEmployee = new Models.Employee
                {
                    Id = id,
                    Surname = Surname,
                    Patronymic = Patronym,
                    Name = Name,
                    PhoneNumber = Telephone,
                    City = City,
                    Street = Street,
                    Zip = ZipCode,
                    DateOfBirth = DateOfBirth,
                    DateOfStart = DateOfStart,
                    Role = Role,
                    Salary = Salary
                };
                await _employeeRepository.EditEmployee(newEmployee);
                return Redirect("/staff");
            }
        }

    }
}
