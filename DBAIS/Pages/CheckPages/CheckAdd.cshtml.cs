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
    public class CheckAddModel : PageModel
    {

        // Form data
        [BindProperty]
        [MaxLength(13)]
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
        [Range(0, 100)]
        public int Vat { get; set; }

        [BindProperty]
        public List<Sale> Sales { get; set; }

        public List<SelectListItem> EmployeeOptions { get; set; }
        public List<SelectListItem> CardsOptions { get; set; }

        private readonly EmployeeRepository _employeeRepository;
        private readonly CheckRepository _checkRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly StoreProductRepository _storeProducts;

        public CheckAddModel(CheckRepository checkRepository, EmployeeRepository employeeRepository, CustomerRepository customerRepository, StoreProductRepository storeProducts)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;
            _storeProducts = storeProducts;
        }

        private async Task InitModel()
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
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await InitModel();
            return Page();
        }

        public async Task<IActionResult> OnGetPrice([FromQuery] string upc)
        {
            try
            {
                var product = await _storeProducts.GetProduct(upc);
                return new JsonResult(new {price = product.Price, count = product.Count});
            }
            catch (EntityNotFoundException<StoreProduct, string>)
            {
                return new JsonResult(null);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await InitModel();
                return Page();
            }
            else
            {
                var newCheck = new Models.Check
                {
                    Number = CheckNumber,
                    CardNum = CardNumber != "" ? CardNumber : null,
                    Date = PrintDate,
                    EmployeeId = IdEmployee,
                    Total = Sales.Sum(s => s.Price * s.Count)*1.2m,
                    Vat = Vat,
                    Sales = Sales
                };
                await _checkRepository.AddCheck(newCheck);
                return Redirect("/checks");
            }
        }
    }
}
