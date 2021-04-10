using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.ClientCardPages
{
    public class ClientCardsPageModel : PageModel
    {
        private readonly CustomerRepository _customerRepository;

        public ClientCardsPageModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Card> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _customerRepository.GetCards(null);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Customers = new List<Card>();
            return Page();
        }
    }
}
