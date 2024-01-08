using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMangement.Data;
using UserMangement.DTOs;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _dataContext;
       public LoginController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> loginRequest(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }
            var User = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password ==loginDto.Password);

            if (User == null)
            {
                return NotFound("User not found");
            }

            // Compare the provided password with the hashed password stored in the database
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, User.Password);

            return Ok("User Added Succesfully");
        }

    }
}
