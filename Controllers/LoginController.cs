using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
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
        public async Task<IActionResult> LoginRequest(LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid input");
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Check if user exists
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Compare the provided password with the hashed password stored in the database
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            // Check if password is invalid
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials");
            }
            // Generate OTP
            string otp = GenerateOTP();

            // Send OTP via Email asynchronously
            await SendOtpEmail(user.Email, otp);

            //await _dataContext.SaveChangesAsync();

            var token = CreateToken(user, otp);

            return Ok(new Utils.Response<AuthToken> { Data = token, Code = "00", Message = "Login Successfully" });
        }

        private AuthToken CreateToken(User user, string otp)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserData", otp), // Add OTP as a claim

            // new Claim(ClaimTypes.UserData, otp),
             new Claim(ClaimType.UserName, user.Name),
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

            return authToken;
        }

        private const string UserDataClaimType = "UserData";

        private string GenerateOTP()
        {
            Random rand = new Random();
            string otp = rand.Next(100000, 999999).ToString();
            return otp;
        }
        private async Task SendOtpEmail(string email, string otp)
        {
            try
            {
                using MailMessage mail = new MailMessage();
                {
                    mail.From = new MailAddress("faisalozigis@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "OTP REGISTRATION CODE";
                    mail.Body = $"Your OTP for registration is: {otp}";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("faisalozigis@gmail.com", "sxqo bnch cxxi eoyk");
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send OTP: {ex.Message}");
            }
        }
    }

}
  
