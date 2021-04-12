using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.YuriiQueryPages
{
    public class YuriiQuery4 : PageModel
    {
        private readonly CheckRepository _checks;

        public YuriiQuery4(CheckRepository checks)
        {
            _checks = checks;
        }

        [FromQuery] public string? CheckNum { get; set; }
        public IList<Check> Checks { get; set; } = ArraySegment<Check>.Empty;

        public async Task OnGetAsync()
        {
            if (CheckNum != null)
                Checks = await _checks.GetTwinChecks(CheckNum);
        }
    }
}