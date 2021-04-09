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
        private readonly ProductsRepository _productRepository;

        public ProductsPageModel(ProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _productRepository.GetProducts();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            /*try
            {
                await _categoryRepository.DeleteCategory(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this category");
            }
            Categories = await _categoryRepository.GetCategoriesAlphabetical();*/
            return Page();
        }
    }
}
