using System.Collections.Generic;
using System.Threading.Tasks;
using DBAIS.Models.DTOs;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAIS.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employee;

        public EmployeeController(EmployeeRepository employee)
        {
            _employee = employee;
        }

        [HttpGet("history")]
        public Task<List<SellingInfo>> GetEmployeeSellingHistory([FromQuery] string employeeId)
        {
            return _employee.GetEmployeeChecks(employeeId);
        }
    }
}
