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
    public class VadymQuery4Model : PageModel
    {

        public List<PromotionalCheck> PromotionalChecks { get; set; }

        private readonly CheckRepository _checkRepository;

        public VadymQuery4Model(CheckRepository checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public async Task OnGetAsync()
        {
            PromotionalChecks = await _checkRepository.GetPromotionalChecks();
        }
    }
}
