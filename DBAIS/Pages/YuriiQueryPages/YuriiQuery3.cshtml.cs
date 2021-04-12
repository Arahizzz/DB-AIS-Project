using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.YuriiQueryPages
{
    public class YuriiQuery3 : PageModel
    {
        private readonly EmployeeRepository _employees;

        public YuriiQuery3(EmployeeRepository employees)
        {
            _employees = employees;
        }

        public IList<CashierInteractionInfo> Favorites { get; set; } = ArraySegment<CashierInteractionInfo>.Empty;
        
        public async Task OnGetAsync()
        {
            Favorites = await _employees.GetFavorites();
        }
    }
}