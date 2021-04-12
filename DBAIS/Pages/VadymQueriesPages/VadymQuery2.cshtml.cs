using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.VadymQueriesPages
{
    public class VadymQuery2Model : PageModel
    {

        public List<BestCategory> BestCategories { get; set; }

        private readonly CategoryRepository _categoryRepository;

        public VadymQuery2Model(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task OnGetAsync()
        {
            BestCategories = await _categoryRepository.GetBestCategories();
        }

    }
}
