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

namespace DBAIS.Pages.StoreProductPages
{
    [Authorize(Roles = "manager")]
    public class StoreProductAddModel : PageModel
    {

        // Form data
        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public int Count { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(12)]
        public string ProductUpc { get; set; }

        [BindProperty]
        [Required]
        public string ProductId { get; set; }

        private readonly StoreProductRepository _storeProductRepository;
        private readonly ProductsRepository _productsRepository;

        public List<SelectListItem> ProductOptions { get; set; }

        public StoreProductAddModel(ProductsRepository productsRepository, StoreProductRepository storeProductRepository)
        {
            _productsRepository = productsRepository;
            _storeProductRepository = storeProductRepository;
        }

        private async Task InitModel()
        {
            var products = await _productsRepository.GetProducts();
            ProductOptions = products.Select(p =>
                                  new SelectListItem
                                  {
                                      Value = p.Id.ToString(),
                                      Text = p.Name
                                  }).ToList();
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            await InitModel();
            return Page();
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
                var newProduct = new StoreProduct
                {
                    Upc = ProductUpc,
                    ProductId = Int32.Parse(ProductId),
                    IsPromotion = false,
                    Price = Price,
                    Count = Count
                };
                await _storeProductRepository.AddProduct(newProduct);
                return Redirect("/storeproducts");
            }
        }
    }
}
