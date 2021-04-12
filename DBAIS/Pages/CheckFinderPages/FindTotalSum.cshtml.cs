using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckFinderPages
{
    [Authorize(Roles = "manager")]
    public class FindTotalSumModel : PageModel
    {
        //ByCashier
        [BindProperty]
        public string? CashierId { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.Now;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Now;

        //Results
        public decimal? PeriodCashiersSum { get; set; }

        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;

        public FindTotalSumModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }
        public void OnGet()
        {

        }
        public async Task OnGetPeriodCashiersSum([FromQuery] string? cashierId, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            CashierId = cashierId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            PeriodCashiersSum = await _checkRepository.GetChecksSum(cashierId, dateFrom, dateTo);
        }
    }
}
