using Microsoft.EntityFrameworkCore;
using AuthenticationFlow.Data;
using AuthenticationFlow.DTOs;
using AuthenticationFlow.Models;
using System.Net.Mail;
using System.Net;

namespace AuthenticationFlow.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> RegisterAsync(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return null;

            var otp = GenerateOTP();
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Otp = otp,
                IsVerified = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SendOTP(user.Email, otp);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash) || !user.IsVerified)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Otp != otp)
                return false;
            user.IsVerified = true;
            user.Otp = string.Empty;
            await _context.SaveChangesAsync();
            return true;
        }

        public string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 10000).ToString();
        }

        public bool SendOTP(string email, string otp)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("irfanilm524@gmail.com");
                message.To.Add(email);
                message.Subject = "OTP for Sign Up";
                message.Body = $"Your OTP for sign up is: {otp}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("irfanilm524@gmail.com", "ordv rayr hjdn mjnm");
                smtp.EnableSsl = true;

                smtp.Send(message);
                smtp.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}
