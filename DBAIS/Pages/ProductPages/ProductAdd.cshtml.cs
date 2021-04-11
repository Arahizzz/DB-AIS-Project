using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBAIS.Pages.ProductPages
{
    [Authorize(Roles = "manager")]
    public class ProductAddeModel : PageModel
    {

        // Form data
        [BindProperty]
        [MinLength(3)]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [BindProperty]
        [MaxLength(100)]
        public string ProductCharacteristics { get; set; }

        [BindProperty]
        [Required]
        public string ProductCategoryId { get; set; }

        private readonly ProductsRepository _productsRepository;
        private readonly CategoryRepository _categoryRepository;

        public List<SelectListItem> CategoryOptions { get; set; }

        public ProductAddeModel(ProductsRepository productsRepository, CategoryRepository categoryRepository)
        {
            _productsRepository = productsRepository;
            _categoryRepository = categoryRepository;
        }

        private async Task InitModel()
        {
            var categories = await _categoryRepository.GetCategoriesAlphabetical();
            var products = await _productsRepository.GetProducts();
            CategoryOptions = categories.Select(c =>
                                  new SelectListItem
                                  {
                                      Value = c.Number.ToString(),
                                      Text = c.Name
                                  }).ToList();
        }
        public async Task<IActionResult> OnGetAsync()
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
                var newProduct = new Models.Product
                {
                    Name = ProductName,
                    Category = new Models.Product.CategoryModel { Number = Int32.Parse(ProductCategoryId) },
                    Characteristics = ProductCharacteristics
                };
                await _productsRepository.AddProduct(newProduct);
                return Redirect("/products");
            }
        }
    }
}
