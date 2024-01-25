using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMangement.Models;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApiResponse _response;

        public CartController(ApiResponse response)
        {
            _response = response;
        }
        [HttpGet("get-cart")]
        public async Task<IActionResult> getproduct(int id)
        {
            return Ok(await _response.CartAsync(id));
        }
        [HttpGet("get-all-cart")]
        public async Task<IActionResult> getAllproduct()
        {
            return Ok(await _response.AllCartAsync());
        }
    }
}
