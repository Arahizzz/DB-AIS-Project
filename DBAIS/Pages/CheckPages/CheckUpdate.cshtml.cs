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

        //public List<SelectListItem> ProductOptions { get; set; }
        //public List<SelectListItem> PromotionOptions { get; set; }

        private readonly EmployeeRepository _employeeRepository;
        private readonly CheckRepository _checkRepository;

        public CheckUpdateModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }

        private async Task InitModel(string upc)
        {
            var employees = await _employeeRepository.GetCashiers(Sort.None);
            EmployeeOptions = employees.Select(e =>
                                  new SelectListItem
                                  {
                                      Value = e.Id,
                                      Text = e.Id
                                  }).ToList();
            /*StoreProduct = await _storeProductRepository.GetProduct(upc);
            var products = await _productsRepository.GetProducts();
            ProductOptions = products.Select(p =>
                                  new SelectListItem
                                  {
                                      Value = p.Id.ToString(),
                                      Text = p.Name,
                                      Selected = p.Id.Equals(StoreProduct.ProductId)
                                  }).ToList();

            var storeProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, false);
            PromotionOptions = storeProducts.Select(p =>
                                  new SelectListItem
                                  {
                                      Value = p.Upc,
                                      Text = p.Name,
                                      Selected = p.Upc.Equals(StoreProduct.UpcPromotional)
                                  }).ToList().Where(x => !x.Value.Equals(StoreProduct.Upc)).ToList();

            IsPromotion = StoreProduct.IsPromotion;*/
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
