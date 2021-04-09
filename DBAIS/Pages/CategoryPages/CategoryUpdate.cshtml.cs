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
    public class CategoryUpdateModel : PageModel
    {
        [FromQuery(Name = "id")]
        public int _categoryId { get; set; }

        public Category _category { get; set; }

        private readonly CategoryRepository _categoryRepository;

        public CategoryUpdateModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        public async void OnGet()
        {
            var categories = await _categoryRepository.GetCategoriesAlphabetical();
            _category = categories.Find(x => x.Number.Equals(_categoryId));
        }
    }
}
