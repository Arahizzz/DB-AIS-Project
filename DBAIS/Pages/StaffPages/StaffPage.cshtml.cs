using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.StaffPages
{
    [Authorize(Roles = "manager")]
    public class StaffPageModel : PageModel
    {

        [BindProperty]
        public string? SelectedSurname { get; set; }

        [BindProperty]
        public string? SelectedRole { get; set; }

        [BindProperty]
        public Sort SelectedSort { get; set; } = Sort.None;

        private readonly EmployeeRepository _employeeRepository;

        public List<Employee> Employee { get; set; }

        public StaffPageModel(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task OnGetAsync()
        {
            Employee = await _employeeRepository.GetCashiers(Sort.None);
        }
        public async Task<IActionResult> OnGetBySurname([FromQuery] string surname)
        {
            SelectedSurname = surname;
            Employee? empl = null;
            Employee = new List<Employee>();
            try
            {
                empl = await _employeeRepository.GetEmployeeBySurname(surname);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Employee by name " + surname + " was not found");
                return Page();
            }
            Employee.Add(empl);
            return Page();
        }

        public async Task OnGetSort([FromQuery] Sort sort)
        {
            SelectedSort = sort;
            Employee = await _employeeRepository.GetCashiers(sort);
        }

    }
}
