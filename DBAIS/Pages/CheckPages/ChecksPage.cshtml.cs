using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckPages
{
    [Authorize(Roles = "cashier, manager")]
    public class ChecksPageModel : PageModel
    {
        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;

        [BindProperty]
        public string? SelectedEmployee { get; set; }

        public ChecksPageModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }

        public List<Check> Checks { get; set; }

        public async Task OnGetAsync()
        {
            Checks = await _checkRepository.GetChecks(null);
        }

        public async Task OnGetByEmployee([FromQuery] string? employeeId)
        {
            SelectedEmployee = employeeId;
            Checks = await _checkRepository.GetChecks(employeeId);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                await _checkRepository.DeleteCheck(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this check");
            }
            Checks = await _checkRepository.GetChecks(null);
            return Page();
        }
    }
    public enum StoreProductSort
    {
        None,
        NameAscending,
        NameDescending,
        CountAscending,
        CountDescending
    }
}
