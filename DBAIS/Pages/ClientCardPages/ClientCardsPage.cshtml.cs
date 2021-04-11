using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.ClientCardPages
{
    [Authorize(Roles = "cashier, manager")]
    public class ClientCardsPageModel : PageModel
    {

        private readonly CustomerRepository _customerRepository;

        [BindProperty]
        public int? SelectedPercent { get; set; }

        [BindProperty]
        public string? SelectedClient { get; set; }

        public ClientCardsPageModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Card> Customers { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _customerRepository.GetCards(null);
        }

        public async Task OnGetByPercent([FromQuery] int? percent)
        {
            SelectedPercent = percent;
            Customers = await _customerRepository.GetCards(percent);
        }

        public async Task OnGetBySurname([FromQuery] string? surname)
        {
            SelectedClient = surname;
            Customers = await _customerRepository.GetCards(null);
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                await _customerRepository.DeleteCustomer(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this card");
            }
            Customers = await _customerRepository.GetCards(null);
            return Page();
        }
    }
}
