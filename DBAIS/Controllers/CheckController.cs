using System.Threading.Tasks;
using DBAIS.Models;
using DBAIS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAIS.Controllers
{
    [Route("[controller]")]
    public class CheckController : ControllerBase
    {
        private readonly CheckRepository _checks;

        public CheckController(CheckRepository checks)
        {
            _checks = checks;
        }

        [HttpPost]
        public async Task<IActionResult> AddCheck([FromBody] Check check)
        {
            await _checks.AddCheck(check);
            return Ok();
        }
    }
}