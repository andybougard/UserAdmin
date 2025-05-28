namespace UserAdmin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using UserAdmin.Data.Entities;
    using UserAdmin.Models.User;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("QRCode")]
        [AllowAnonymous]
        public async Task<IActionResult> getQRCode(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound("User not found");

            var secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
            var email = user.Email;
            var qrCodeUrl = $"otpauth://totp/{email}?secret={secretKey}&issuer=UserAdmin";
            return Ok(qrCodeUrl);
        }

        [HttpPost("Enable2FA")]
        public async Task<IActionResult> Set2FA(string userName, bool enable)
        {
            var requestUser = await _userManager.FindByNameAsync(userName);
            if (requestUser == null)
                return NotFound("User not found");

            var update2FA = await _userManager.SetTwoFactorEnabledAsync(requestUser, enable);
            if (update2FA.Succeeded)
                return Ok("2FA status updated");
            else
                return BadRequest("Failed to update 2FA status");
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel userRequest)
        {
            if (userRequest == null || string.IsNullOrEmpty(userRequest.Email) || string.IsNullOrEmpty(userRequest.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            AppUser user = new AppUser
            {
                UserName = userRequest.Email,
                Email = userRequest.Email,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                DateOfBirth = userRequest.DateOfBirth,
                Address = userRequest.Address,
                City = userRequest.City
            };
            var result = await _userManager.CreateAsync(user, userRequest.Password);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Message = "User registered successfully",
                });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginModel userRequest)
        {
            if (userRequest == null || string.IsNullOrEmpty(userRequest.Email) || string.IsNullOrEmpty(userRequest.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            var user = await _userManager.FindByEmailAsync(userRequest.Email);
            if (user == null)
                return NotFound(new { Message = "User not found" });


            bool result = await _userManager.CheckPasswordAsync(user, userRequest.Password);
            if (result)
            {
                var loginKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Appsettings:JWTSecret"]!));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.FirstName!),
                        new Claim(ClaimTypes.Surname, user.LastName!),
                        new Claim(ClaimTypes.Email, user.Email!)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(loginKey, SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                Response.Headers.Append("Authorization", tokenHandler.WriteToken(token));

                return Ok(new { Message = "Login successful" });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
        }
    }
}