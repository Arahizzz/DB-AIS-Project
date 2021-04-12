using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.VadymQueriesPages
{
    public class VadymQuery1Model : PageModel
    {

        [BindProperty]
        public string SelectedCashier { get; set; }

        public List<SellingInfo> CashierHistory { get; set; }

        private readonly EmployeeRepository _employeeRepository;

        public VadymQuery1Model(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void OnGet()
        {
            CashierHistory = new List<SellingInfo>();
        }

        public async Task OnGetByCashier([FromQuery] string cashierId)
        {
            SelectedCashier = cashierId;
            CashierHistory = new List<SellingInfo>();
            if (ModelState.IsValid)
            {
                var history = await _employeeRepository.GetEmployeeChecks(cashierId);
                if (history == null || history.Count < 1)
                {
                    ModelState.AddModelError("", "Cashier by " + cashierId + " was not found");
                }
                else
                {
                    CashierHistory.AddRange(history);
                }
            }
        }
    }
}
