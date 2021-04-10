using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.ProductPages
{
    public class ProductsPageModel : PageModel
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductsRepository _productRepository;

        [BindProperty]
        public string SelectedCategory { get; set; } = String.Empty;
        [BindProperty]
        public Sort? SelectedSort { get; set; }

        public ProductsPageModel(ProductsRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Product> Products { get; set; }
        public List<Models.Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _productRepository.GetProducts();
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
        }

        public async Task OnGetByCategory([FromQuery] string? category, [FromQuery] Sort? sort)
        {
            SelectedCategory = category;
            SelectedSort = sort;
            Products = await _productRepository.GetProducts(category, sort.GetValueOrDefault(Sort.Ascending));
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProduct(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this product");
            }
            Products = await _productRepository.GetProducts();
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
            return Page();
        }
    }
}
