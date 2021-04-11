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
    public class CheckUpdateModel : PageModel
    {

        // Current check
        public Check CurrentCheck { get; set; }

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
        public List<SelectListItem> CardsOptions { get; set; }


        private readonly EmployeeRepository _employeeRepository;
        private readonly CheckRepository _checkRepository;
        private readonly CustomerRepository _customerRepository;

        public CheckUpdateModel(CheckRepository checkRepository, EmployeeRepository employeeRepository, CustomerRepository customerRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
        }

        private async Task InitModel(string id)
        {
            var employees = await _employeeRepository.GetCashiers(Sort.None);
            EmployeeOptions = employees.Select(e =>
                                  new SelectListItem
                                  {
                                      Value = e.Id,
                                      Text = e.Id
                                  }).ToList();
            var cards = await _customerRepository.GetCards(null);
            CardsOptions = new List<SelectListItem>();
            CardsOptions.Add(new SelectListItem
            {
                Value = "",
                Text = "-"
            });
            CardsOptions.AddRange(cards.Select(c =>
                                  new SelectListItem
                                  {
                                      Value = c.Number,
                                      Text = c.Number
                                  }).ToList());
            var checks = await _checkRepository.GetChecks(null, null, null);
            CurrentCheck = checks.Find(x => x.Number.Equals(id));
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id);
            if (CurrentCheck == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateSale([FromForm] string saleUpc, [FromForm] int count, [FromForm] decimal price)
        {
            //await InitModel(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteSale([FromForm] string id)
        {
            //Sales.Add(new Sale { Check = "check", Upc = saleUpc, Count = count, Price = price });
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
                /*var newProduct = new StoreProduct
                {
                    Upc = id,
                    ProductId = Int32.Parse(ProductId),
                    IsPromotion = IsPromotion,
                    UpcPromotional = IsPromotion ? UpcPromotional : null,
                    Price = Price,
                    Count = Count
                };
                await _storeProductRepository.UpdateProduct(newProduct);*/
                return Redirect("/storeproducts");
            }
        }
    }
}
