using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckFinderPages
{
    public class FindTotalSumModel : PageModel
    {
        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;

        [BindProperty]
        public string? SelectedCashier { get; set; }

        public FindTotalSumModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }
        public void OnGetPeriodCashierSum()
        {
            //_checkRepository.
        }
    }
}
