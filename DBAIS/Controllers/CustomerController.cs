using System.Threading.Tasks;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAIS.Controllers
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customers;

        public CustomerController(CustomerRepository customers)
        {
            _customers = customers;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetCustomerPurchaseHistory([FromQuery] string cardNum)
        {
            var list = await _customers.GetCustomerChecks(cardNum);
            return Ok(list);
        }
    }
}