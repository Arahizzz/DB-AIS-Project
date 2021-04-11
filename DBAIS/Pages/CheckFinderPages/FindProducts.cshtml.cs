using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckFinderPages
{
    public class FindProductsModel : PageModel
    {
        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;

        [BindProperty]
        public string? SelectedCheck { get; set; }

        public FindProductsModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }
        public void OnGet()
        {
            //_checkRepository.
        }
    }
}
