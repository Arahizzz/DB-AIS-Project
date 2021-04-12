using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.VadymQueriesPages
{
    public class VadymQuery3Model : PageModel
    {
        [BindProperty]
        public int SelectedMonth { get; set; } = (int)MonthName.Jan;

        public EmployeeOfTheMonth? EmployeeOfTheMonth { get; set; }

        private readonly EmployeeRepository _employeeRepository;

        public VadymQuery3Model(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void OnGet()
        {
        }

        public async Task OnGetBestEmployee([FromQuery] int month)
        {
            SelectedMonth = month;
            if (ModelState.IsValid)
            {
                var empl = await _employeeRepository.GetEmployeeOfTheMonth(month);
                if (empl == null)
                {
                    ModelState.AddModelError("", "No employee of the month found");
                }
                else
                {
                    EmployeeOfTheMonth = empl;
                }
            }
        }

    }
    public enum MonthName { Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
}

