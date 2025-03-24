using BL;
using Microsoft.AspNetCore.Mvc;

namespace Gui.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BagController : ControllerBase
    {
        private readonly BagCalc _bagCalc;

        public BagController(BagCalc bagCalc)
        {
            _bagCalc = bagCalc;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> Calculate()
        {
            var result = await _bagCalc.Calculate();
            return Ok(result);
        }
    }
}
