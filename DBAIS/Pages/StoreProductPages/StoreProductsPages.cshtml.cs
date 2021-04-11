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

        [BindProperty]
        public string? SelectedCategory { get; set; } = String.Empty;
        [BindProperty]
        public bool IsPromotional { get; set; }
        
        [BindProperty]
        public StoreProductSort SelectedSort { get; set; } = StoreProductSort.None;

        public StoreProductsPagesModel(StoreProductRepository storeProductRepository, CategoryRepository categoryRepository)
        {
            _storeProductRepository = storeProductRepository;
            _categoryRepository = categoryRepository;
        }

        public List<StoreProductDto> StoreProducts { get; set; }

        public async Task OnGetAsync()
        {
            StoreProducts = await _storeProductRepository.GetStoreProductsInfo(Sort.None, Sort.None, null);
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
