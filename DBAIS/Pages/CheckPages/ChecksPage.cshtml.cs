using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckPages
{
    [Authorize(Roles = "cashier, manager")]
    public class ChecksPageModel : PageModel
    {

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Now;

        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly UserManager<EmployeeUser> _userManager;

        public EmployeeUser? UserInfo {get; set;}

        [BindProperty]
        public string? SelectedEmployee { get; set; }

        public ChecksPageModel(CheckRepository checkRepository, EmployeeRepository employeeRepository, UserManager<EmployeeUser> userManager)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public List<Check> Checks { get; set; }

        public async Task OnGetAsync()
        {
            if (User.IsInRole("cashier"))
            {
                UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
                Checks = await _checkRepository.GetChecks(UserInfo.Id, null, null);
            }
            else if (User.IsInRole("manager"))
            {
                Checks = await _checkRepository.GetChecks(null, null, null);
            }
        }

        public async Task OnGetByPeriod([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (User.IsInRole("cashier"))
            {
                DateFrom = dateFrom;
                DateTo = dateTo;
                UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);
                Checks = await _checkRepository.GetChecks(UserInfo.Id, dateFrom, dateTo);
            }
            else if (User.IsInRole("manager"))
            {
                Checks = await _checkRepository.GetChecks(null, null, null);
            }
        }

        public async Task OnGetByEmployee([FromQuery] string? employeeId)
        {
            SelectedEmployee = employeeId;
            Checks = await _checkRepository.GetChecks(employeeId, null, null);
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
            Checks = await _checkRepository.GetChecks(null, null, null);
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
