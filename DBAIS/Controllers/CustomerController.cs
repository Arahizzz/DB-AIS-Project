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
    }
}