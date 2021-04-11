using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    [Authorize(Roles = "manager")]
    public class CategoryAddModel : PageModel
    {

        [BindProperty]
        [MinLength(3)]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        private readonly CategoryRepository _categoryRepository;

        public CategoryAddModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var newCategory = new Models.Category { Name = CategoryName };
                await _categoryRepository.AddCategory(newCategory);
                return Redirect("/category");
            }
        }
    }
}
