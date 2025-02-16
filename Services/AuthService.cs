using AvitoTestTask.Data;
using AvitoTestTask.Helpers;
using AvitoTestTask.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AvitoTestTask.Services
{
    public class AuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return _jwtHelper.GenerateToken(user);
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
            {
                return false;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User(username, password);

            await _userRepository.AddAsync(newUser);
            return true;
        }
    }
}
