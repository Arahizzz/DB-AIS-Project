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
    [Authorize(Roles = "cashier, manager")]
    public class CheckUpdateModel : PageModel
    {

        // Current check
        public Check CurrentCheck { get; set; }

        // Form data
        [BindProperty]
        [MaxLength(13)]
        public string? CardNumber { get; set; }


        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime PrintDate { get; set; }


        public List<SelectListItem> EmployeeOptions { get; set; }
        public List<SelectListItem> CardsOptions { get; set; }


        private readonly EmployeeRepository _employeeRepository;
        private readonly CheckRepository _checkRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly StoreProductRepository _storeProductRepository;
        private readonly UserManager<EmployeeUser> _userManager;


        public EmployeeUser UserInfo { get; set; }

        public CheckUpdateModel(CheckRepository checkRepository, EmployeeRepository employeeRepository, CustomerRepository customerRepository, UserManager<EmployeeUser> userManager, StoreProductRepository storeProductRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _storeProductRepository = storeProductRepository;
        }

        private async Task InitModel(string id)
        {
            UserInfo = await _userManager.FindByNameAsync(User.Identity.Name);

            var checks = await _checkRepository.GetChecks(null, null, null);
            CurrentCheck = checks.Find(x => x.Number.Equals(id));

            var employees = await _employeeRepository.GetCashiers(Sort.None);
            EmployeeOptions = employees.Select(e =>
                                  new SelectListItem
                                  {
                                      Value = e.Id,
                                      Text = e.Id
                                  }).ToList();
            var cards = await _customerRepository.GetCards(null);
            CardsOptions = new List<SelectListItem>();
            CardsOptions.AddRange(cards.Select(c =>
                                  new SelectListItem
                                  {
                                      Value = c.Number,
                                      Text = c.Number,
                                      Selected = c.Number.Equals(CurrentCheck.CardNum)
                                  }).ToList());
            PrintDate = CurrentCheck.Date;
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

        public async Task<IActionResult> OnPostCreateSale([FromForm] string id, [FromForm] string upc, [FromForm] int count)
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
            try
            {
                var prod = await _storeProductRepository.GetProduct(upc);
                if (prod.Count < count)
                {
                    ModelState.AddModelError("", "There are only " + prod.Count + " units of " + prod.Upc + " product in the storage");
                    return Page();
                }
                var newSale = new Sale
                {
                    Check = id,
                    Upc = upc,
                    Count = count,
                    Price = prod.Price
                };
                CurrentCheck.Sales.Add(newSale);
                var newTotal = CurrentCheck.Sales.Sum(x => x.Price * x.Count);
                CurrentCheck.Total = newTotal;
            
                await _checkRepository.UpdateCheck(CurrentCheck);
            }
            catch
            {
                ModelState.AddModelError("","Wrong upc entered");
            }
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
            var newTotal = CurrentCheck.Sales.Sum(x => x.Price*x.Count);
            CurrentCheck.Total = newTotal;
            await _checkRepository.UpdateCheck(CurrentCheck);
            await InitModel(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] string id)
        {
            await InitModel(id);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newTotal = CurrentCheck.Sales.Sum(x => x.Price * x.Count);
                var newCheck = new Check
                {
                    CardNum = CardNumber != "" ? CardNumber : null,
                    Date = PrintDate,
                    Sales = CurrentCheck.Sales,
                    EmployeeId = CurrentCheck.EmployeeId,
                    Number = id,
                    Total = newTotal,
                    Vat = CurrentCheck.Vat
                };
                await _checkRepository.UpdateCheck(newCheck);
                return Redirect("/checks");
            }
        }
    }
}
