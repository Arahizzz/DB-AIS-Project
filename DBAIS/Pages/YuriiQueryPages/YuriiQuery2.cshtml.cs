using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBAIS.Pages.YuriiQueryPages
{
    public class YuriiQuery2 : PageModel
    {
        private readonly ProductsRepository _products;

        public YuriiQuery2(ProductsRepository products)
        {
            _products = products;
        }

        public IList<ProductRevenueInfo> Revenues { get; set; } = ArraySegment<ProductRevenueInfo>.Empty;
        
        public async Task OnGetAsync()
        {
            Revenues = await _products.GetMostProfitableProducts();
        }
    }
}