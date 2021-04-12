using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckFinderPages
{
    public class FindProductsModel : PageModel
    {
        private readonly CheckRepository _checkRepository;
        private readonly EmployeeRepository _employeeRepository;

        [FromQuery]
        public string? SelectedCheck { get; set; }

        public IList<PurchaseInfo.ProductInfo> Products { get; set; } = ArraySegment<PurchaseInfo.ProductInfo>.Empty;

        public FindProductsModel(CheckRepository checkRepository, EmployeeRepository employeeRepository)
        {
            _checkRepository = checkRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task OnGetAsync()
        {
            if (SelectedCheck != null)
                Products = await _checkRepository.GetCheckProducts(SelectedCheck);
        }
    }
}
