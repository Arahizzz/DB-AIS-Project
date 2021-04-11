using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    [Authorize(Roles = "cashier, manager")]
    public class CategoryPageModel : PageModel
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryPageModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Models.Category> Categories {get; set;}

        public async Task OnGetAsync()
        {
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategory(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "You can`t delete this category");
            }
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
            return Page();
        }
    }
}
