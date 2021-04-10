using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.ClientCardPages
{
    public class ClientCardAddModel : PageModel
    {

        // Form data
        [BindProperty]
        [MaxLength(13)]
        public string CardNumber { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Surname { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Name { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string Patronym { get; set; }

        [BindProperty]
        [MaxLength(13)]
        public string Telephone { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string? City { get; set; }

        [BindProperty]
        [MaxLength(50)]
        public string? Street { get; set; }

        [BindProperty]
        [MaxLength(9)]
        public string? ZipCode { get; set; }

        [BindProperty]
        [Range(1, 100)]
        public int Percent { get; set; }

        private readonly CustomerRepository _customerRepository;

        public ClientCardAddModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newCard = new Models.Card
                {
                    Number = CardNumber,
                    Surname = Surname,
                    Patronymic = Patronym,
                    Name = Name,
                    Phone = Telephone,
                    City = City,
                    Street = Street,
                    Zip = ZipCode,
                    Percent = Percent
                };
                await _customerRepository.AddCard(newCard);
                return Redirect("/customers");
            }
        }
    }
}
