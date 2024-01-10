using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMangement.Data;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CustomerController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get-customer"), Authorize(Roles ="Admin")]

        public async Task<IActionResult> GetCustomer()
        {
            return Ok();
        }
    }
}
