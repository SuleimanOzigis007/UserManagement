using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMangement.Data;
using UserMangement.Models;

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

        public IEnumerable<string> GetCustomer()
        {
            return new string[] { "John Doe", "Jane Doe" };
        }
    }
}
