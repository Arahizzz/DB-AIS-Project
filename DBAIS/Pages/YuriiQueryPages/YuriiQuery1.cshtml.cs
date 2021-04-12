using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.YuriiQueryPages
{
    public class YuriiQuery1 : PageModel
    {
        private readonly CheckRepository _checks;

        public YuriiQuery1(CheckRepository checks)
        {
            _checks = checks;
        }

        [FromQuery] public string? CustomerCard { get; set; }
        public IList<PurchaseInfo> History { get; set; } = ArraySegment<PurchaseInfo>.Empty;
        
        public async Task OnGetAsync()
        {
            if (CustomerCard != null)
                History = await _checks.GetCustomerChecks(CustomerCard);
        }
    }
}