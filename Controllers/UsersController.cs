using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationFlow.DTOs;
using AuthenticationFlow.Models;
using AuthenticationFlow.Services;

namespace AuthenticationFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            if (result == null)
                return BadRequest("Username already exists.");

            // After registration, inform client to redirect to OTP verification
            return Ok(new { message = "Registration successful. OTP sent to your email. Please verify.", email = dto.Email });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var verified = await _service.VerifyOtpAsync(dto.Email, dto.Otp);
            if (!verified)
                return BadRequest("Invalid OTP or user not found.");
            return Ok(new { message = "OTP verified. You can now log in." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            if (result == null)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(result);
            return Ok(new { user = result, token });
        }

        [HttpPost("request-otp")]
        public IActionResult RequestOtp([FromBody] string email)
        {
            var otp = _service.GenerateOTP();
            var sent = _service.SendOTP(email, otp);
            if (sent)
            {
                // For demo, return OTP in response (in production, do not return OTP)
                return Ok(new { message = "OTP sent successfully.", otp });
            }
            else
            {
                return StatusCode(500, "Failed to send OTP.");
            }
        }

        private string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"]);

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Username)
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
