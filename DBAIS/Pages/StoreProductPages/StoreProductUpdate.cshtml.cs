using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBAIS.Pages.StoreProductPages
{
    [Authorize(Roles = "manager")]
    public class StoreProductUpdateModel : PageModel
    {

        // Current product

        public StoreProduct StoreProduct { get; set; }

        // Form data
        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public int Count { get; set; }

        [BindProperty]
        public bool IsPromotion { get; set; }

        [BindProperty]
        [Required]
        public string? UpcPromotional { get; set; }

        [BindProperty]
        [Required]
        public string ProductId { get; set; }

        private readonly StoreProductRepository _storeProductRepository;
        private readonly ProductsRepository _productsRepository;

        public List<SelectListItem> ProductOptions { get; set; }
        public List<SelectListItem> PromotionOptions { get; set; }

        public StoreProductUpdateModel(ProductsRepository productsRepository, StoreProductRepository storeProductRepository)
        {
            _productsRepository = productsRepository;
            _storeProductRepository = storeProductRepository;
        }

        private async Task InitModel(string upc)
        {
            StoreProduct = await _storeProductRepository.GetProduct(upc);
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

            IsPromotion = StoreProduct.IsPromotion;
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id);
            if (StoreProduct == null)
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
                var newProduct = new StoreProduct
                {
                    Upc = id,
                    ProductId = Int32.Parse(ProductId),
                    IsPromotion = IsPromotion,
                    UpcPromotional = IsPromotion ? UpcPromotional : null,
                    Price = Price,
                    Count = Count
                };
                await _storeProductRepository.UpdateProduct(newProduct);
                return Redirect("/storeproducts");
            }
        }
    }
}
