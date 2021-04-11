using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBAIS.Pages.CheckPages
{
    [Authorize(Roles = "manager")]
    public class CheckAddModel : PageModel
    {

        // Form data
        [BindProperty]
        [MaxLength(10)]
        public string? CardNumber { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string CheckNumber { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string IdEmployee { get; set; }

        [BindProperty]
        public DateTime PrintDate { get; set; }

        [BindProperty]
        [Range(0, int.MaxValue)]
        public int Total { get; set; }

        [BindProperty]
        [Range(0, 100)]
        public int Vat { get; set; }

        public List<SelectListItem> EmployeeOptions { get; set; }

        private readonly EmployeeRepository _employeeRepository;
        private readonly CheckRepository _checkRepository;

        public CheckAddModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }

        private async Task InitModel()
        {
            var employees = await _employeeRepository.GetCashiers(Sort.None);
            EmployeeOptions = employees.Select(e =>
                                  new SelectListItem
                                  {
                                      Value = e.Id,
                                      Text = e.Id
                                  }).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await InitModel();
                return Page();
            }
            else
            {
                var newCheck = new Models.Check
                {
                    Number = CheckNumber,
                    CardNum = CardNumber,
                    Date = PrintDate,
                    EmployeeId = IdEmployee,
                    Total = Total,
                    Vat = Vat
                };
                await _checkRepository.AddCheck(newCheck);
                return Redirect("/checks");
            }
        }
    }
}
