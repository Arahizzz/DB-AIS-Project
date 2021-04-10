using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBAIS.Pages.ProductPages
{
    public class ProductUpdateModel : PageModel
    {
        
        // Current product
        
        public Models.Product Product { get; set; }
        
        // Form data
        [BindProperty]
        [MinLength(3)]
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

        public ProductUpdateModel(ProductsRepository productsRepository, CategoryRepository categoryRepository)
        {
            _productsRepository = productsRepository;
            _categoryRepository = categoryRepository;
        }

        private async Task InitModel(int id)
        {
            var categories = await _categoryRepository.GetCategoriesAlphabetical();
            var products = await _productsRepository.GetProducts();
            Product = products.Find(x => x.Id.Equals(id));
            CategoryOptions = categories.Select(c =>
                                  new SelectListItem
                                  {
                                      Value = c.Number.ToString(),
                                      Text = c.Name,
                                      Selected = c.Number.Equals(Product.Category.Number)
                                  }).ToList();
            ProductCharacteristics = Product.Characteristics;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await InitModel(id.GetValueOrDefault());
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                await InitModel(id);
                return Page();
            }
            else
            {
                var newProduct = new Models.Product { 
                    Id = id,
                    Name = ProductName,
                    Category = new Models.Product.CategoryModel { Number = Int32.Parse(ProductCategoryId) },
                    Characteristics = ProductCharacteristics
                };
                await _productsRepository.UpdateProduct(newProduct);
                return Redirect("/products");
            }
        }
    }
}
