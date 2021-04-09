using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CheckRepository _customerRepository;

        public IndexModel(CheckRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<PurchaseInfo> Purchases { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Purchases = await _customerRepository.GetCustomerChecks("7121520341739");
        }
    }
}
