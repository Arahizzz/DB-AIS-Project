using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages
{
    public class CategoryPageModel : PageModel
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryPageModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> Categories {get; set;}

        public async Task OnGetAsync()
        {
            Categories = await _categoryRepository.GetCategoriesAlphabetical();
        }
    }
}
