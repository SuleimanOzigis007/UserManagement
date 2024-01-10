using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMangement.Data;
using UserMangement.Models;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get")]

        public async Task<IActionResult>  GetUsers()
        {
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetUserId(int Id)
        {
            var user = await _dataContext.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> create([FromBody] User model)
            {

            if (!IsValidEmailFormat(model.Email))
            {
                return BadRequest("Invalid email format");
            }

            if (!ContainsSpecialCharacters(model.Password))
            {
                return BadRequest("Password must contain special characters");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashedPassword;

            _dataContext.Users.Add(model);
            await _dataContext.SaveChangesAsync();

            var responseModel = new
            {
                model.Id,
                model.Email,
                model.Name
                // Include other properties you want to return
            };
            return Ok(new { message = "User Added Succesfully ", responseModel, hashedPassword =hashedPassword, });
        }

        [HttpDelete]
        [Route("delete")]

        public async Task<IActionResult> Delete(int Id)
        {
            var user = await _dataContext.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();

            var res = new Response<User> { Message = "User Deleted Succesfully", Code = "407" };

            return Ok(res);
        }

        private class Response<T>
        {
            public  string Code { get; set; }
            public string Data { get; set; }
            public string Message { get; set; }

        }

        private bool ContainsSpecialCharacters(string password)
        {
            string specialCharacters = "!@#$%^&*()_+{}[]-=';:/?.,><";

            // Check if the password contains any of the special characters
            return password.Any(ch => specialCharacters.Contains(ch));
        }

        private bool IsValidEmailFormat(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
