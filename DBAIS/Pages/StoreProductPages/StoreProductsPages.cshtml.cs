using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.StoreProductPages
{
    [Authorize(Roles = "cashier, manager")]
    public class StoreProductsPagesModel : PageModel
     {
        private readonly CategoryRepository _categoryRepository;
        private readonly StoreProductRepository _storeProductRepository;
        private readonly ProductsRepository _productsRepository;

        [BindProperty]
        public string? SelectedUpc { get; set; } = String.Empty;
        [BindProperty]
        public string? SelectedCategory { get; set; } = String.Empty;
        [BindProperty]
        public bool IsPromotional { get; set; }
        
        [BindProperty]
        public StoreProductSort SelectedSort { get; set; } = StoreProductSort.None;

        public StoreProductsPagesModel(StoreProductRepository storeProductRepository, CategoryRepository categoryRepository, ProductsRepository productsRepository)
        {
            _storeProductRepository = storeProductRepository;
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
        }

        public List<StoreProductDto> StoreProducts { get; set; }

        public async Task OnGetAsync()
        {
            StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, null);
        }
        public async Task<IActionResult> OnGetByUpc([FromQuery] string upc)
        {
            SelectedUpc = upc;
            StoreProducts = new List<StoreProductDto>();
            StoreProduct p = null;
            try
            {
                p = await _storeProductRepository.GetProduct(upc);
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Store product by UPC " + upc + " was not found");
                return Page();
            }
            
            var prods = await _productsRepository.GetProducts();
            var prod = prods.Find(x => x.Id.Equals(p.ProductId));
            var product = new StoreProductDto
            {
                Upc = p.Upc,
                Name = prod != null ? prod.Name : "",
                IsPromotion = p.IsPromotion,
                Price = p.Price,
                Count = p.Count
            };
            StoreProducts.Add(product);
            return Page();
        }
        public async Task OnGetByPromotional([FromQuery] bool isPromotional, [FromQuery] StoreProductSort sort)
        {
            SelectedSort = sort;
            IsPromotional = isPromotional;
            switch (sort)
            {
                case StoreProductSort.None:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, isPromotional);
                    break;
                case StoreProductSort.NameAscending:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.Ascending, Sort.None, isPromotional);
                    break;
                case StoreProductSort.NameDescending:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.Descending, Sort.None, isPromotional);
                    break;
                case StoreProductSort.CountAscending:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.Ascending, isPromotional);
                    break;
                case StoreProductSort.CountDescending:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.Descending, isPromotional);
                    break;
                default:
                    StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, isPromotional);
                    break;
            };
        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                await _storeProductRepository.DeleteProduct(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this product");
            }
            StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, null);
            return Page();
        }
     }
    public enum StoreProductSort
    {
        None,
        NameAscending,
        NameDescending,
        CountAscending,
        CountDescending
    }
}
