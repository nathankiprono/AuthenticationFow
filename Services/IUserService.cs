using AuthenticationFlow.DTOs;

namespace AuthenticationFlow.Services
{
    public interface IUserService
    {
        Task<UserDto?> RegisterAsync(CreateUserDto dto);
        Task<UserDto?> LoginAsync(LoginDto dto);
        string GenerateOTP();
        bool SendOTP(string email, string otp);
        Task<bool> VerifyOtpAsync(string email, string otp);
    }
}
