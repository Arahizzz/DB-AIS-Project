using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<EmployeeUser> _userManager;

        public EmployeeUser UserInfo { get; set; }

        public CheckUpdateModel(CheckRepository checkRepository, EmployeeRepository employeeRepository, CustomerRepository customerRepository, UserManager<EmployeeUser> userManager)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
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

        public async Task<IActionResult> OnPostCreateSale([FromForm] string id, [FromForm] string upc, [FromForm] int count, [FromForm] decimal price)
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
            var newSale = new Sale
            {
                Check = id,
                Upc = upc,
                Count = count,
                Price = price
            };
            CurrentCheck.Sales.Add(newSale);
            var newTotal = CurrentCheck.Sales.Sum(x => x.Price);
            CurrentCheck.Total = newTotal;
            await _checkRepository.UpdateCheck(CurrentCheck);
            await InitModel(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteSale([FromForm] string id, [FromForm] string upc)
        {
            await _checkRepository.DeleteProductFromCheck(id, upc);
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id);
            if (CurrentCheck == null)
            {
                return NotFound();
            }
            var newTotal = CurrentCheck.Sales.Sum(x => x.Price);
            CurrentCheck.Total = newTotal;
            await _checkRepository.UpdateCheck(CurrentCheck);
            await InitModel(id);
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
                return Redirect("/checks");
            }
        }
    }
}
