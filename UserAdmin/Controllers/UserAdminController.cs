
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserAdmin.Data.Entities;
using UserAdmin.Models.User;

namespace UserAdmin.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserAdminController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public UserAdminController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Route("users")]
    public IActionResult GetUsers()
    {
        try
        {
            List<UserInfoResponseModel> users = _userManager.Users.Where(u => u.UserName != "fernando.orfra@hotmail.com").Select(u => new UserInfoResponseModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.AccessFailedCount <= 5
            }).ToList();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving users", Error = ex.Message });
        }
    }

    [HttpGet]
    [Route("users/{email}")]
    public IActionResult GetUserByEmail(string email)
    {
        try
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var userInfo = new UserInfoResponseModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.AccessFailedCount <= 5
            };

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving the user", Error = ex.Message });
            
        }
    }

    [HttpPost]
    [Route("users")]
    public IActionResult CreateUser([FromBody] UserRegistrationModel user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            AppUser newUser = new AppUser
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                City = user.City
            };

            var result = _userManager.CreateAsync(newUser, user.Password).Result;
            if (result.Succeeded)
            {
                return Ok(new { Message = "User created successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to create user", result.Errors });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the user", Error = ex.Message });
        }
    }
}