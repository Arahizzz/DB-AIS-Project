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
        private readonly CustomerRepository _customerRepository;

        public IndexModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<PurchaseInfo> Purchases {get; set;}

        public async Task OnGetAsync()
        {
            Purchases = await _customerRepository.GetCustomerChecks("7121520341739");
        }
    }
}
