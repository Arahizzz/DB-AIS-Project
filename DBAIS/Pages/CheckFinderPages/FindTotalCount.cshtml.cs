using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.CheckFinderPages
{
    [Authorize(Roles = "manager")]
    public class FindTotalCountModel : PageModel
    {
        //ByCashier
        [BindProperty]
        public string ProductUpc { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.Now;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Now;

        //Results
        public Int64? TotalCountPeriod { get; set; }

        private readonly StoreProductRepository _storeProductRepository;
        private readonly CheckRepository _checkRepository;

        public FindTotalCountModel(CheckRepository checkRepository, StoreProductRepository storeProductRepository)
        {
            _storeProductRepository = storeProductRepository;
            _checkRepository = checkRepository;
        }
        public void OnGet()
        {

        }
        public async Task OnGetPeriodTotalCount([FromQuery] string upc, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            ProductUpc = upc;
            DateFrom = dateFrom;
            DateTo = dateTo;
            if (ModelState.IsValid)
            {
                TotalCountPeriod = await _checkRepository.GetProductCount(upc, dateFrom, dateTo);
            }
            else TotalCountPeriod = null;
        }
    }
}
