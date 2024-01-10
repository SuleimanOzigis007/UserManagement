using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMangement.Data;
using UserMangement.DTOs;
using UserMangement.Models;
using UserMangement.Utils;

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
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid input");
            }

            var User = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Check if user exists
            if (User == null)
            {
                return NotFound("User not found");
            }

            // Compare the provided password with the hashed password stored in the database
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, User.Password);

            // Check if password is invalid
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials");
            }

            string token = CreateToken(User);

             var r = new Utils.Response<LoginDto> { Data = loginDto, Code = new StatusCode().SUCCESS, Message = $"Login Successful {token}" };

          
            return Ok(r);

        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimType.OTP, user.Email),
            new Claim(ClaimTypes.Role, "Admin"),

            };

            DateTime expireAt = DateTime.Now.AddMinutes(5);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5121",
                audience: "http://localhost:5121",
                claims: claims,
                expires: expireAt,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            var authToken = new AuthToken { ExpireAt = $"{expireAt}", Token = tokenString };

            return tokenString;
        }
    }
 
}
  
