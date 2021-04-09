using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    public class CategoryAddModel : PageModel
    {
        private readonly CategoryRepository _categoryRepository;
        public string Message { get; set; } = string.Empty;

        public CategoryAddModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void OnGet()
        {
            Message = "Get";
        }
        public async Task<IActionResult> OnPostAsync([FromForm] string categoryName)
        {
            Message = "Post";
            var newCategory = new Category { Name = categoryName };
            await _categoryRepository.AddCategory(newCategory);
            return Redirect("/category");
        }
    }
}
