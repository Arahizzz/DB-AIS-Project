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

namespace DBAIS.Pages
{
    [Authorize(Roles = "manager")]
    public class CategoryUpdateModel : PageModel
    {

        [BindProperty]
        public Models.Category Category { get; set; }

        [BindProperty]
        [MinLength(3)]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        private readonly CategoryRepository _categoryRepository;

        public CategoryUpdateModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetCategoriesAlphabetical();
            Category = categories.Find(x => x.Number.Equals(id));
            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] int id)
        {
            var categories = await _categoryRepository.GetCategoriesAlphabetical();
            Category = categories.Find(x => x.Number.Equals(id));
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newCategory = new Models.Category { Number = id, Name = CategoryName };
                await _categoryRepository.UpdateCategory(newCategory);
                return Redirect("/category");
            }
        }
    }
}
