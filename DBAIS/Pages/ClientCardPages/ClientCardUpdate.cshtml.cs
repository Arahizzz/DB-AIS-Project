using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.ClientCardPages
{
    [Authorize(Roles = "cashier, manager")]
    public class ClientCardUpdateModel : PageModel
    {

        // Current product
        public Models.Card Card { get; set; }

        // Form data

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

        public ClientCardUpdateModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private async Task InitModel(string number)
        {
            var cards = await _customerRepository.GetCards(null);
            Card = cards.Find(x => x.Number.Equals(number));
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id);
            if (Card == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                await InitModel(id);
                return Page();
            }
            else
            {
                var newCard = new Models.Card
                {
                    Number = id,
                    Surname = Surname,
                    Patronymic = Patronym,
                    Name = Name,
                    Phone = Telephone,
                    City = City,
                    Street = Street,
                    Zip = ZipCode,
                    Percent = Percent
                };
                await _customerRepository.EditCard(newCard);
                return Redirect("/customers");
            }
        }
    }
}
