using Microsoft.AspNetCore.Mvc;
using ContestSystem.Data;
using ContestSystem.DTOs;
using ContestSystem.Helpers;
using ContestSystem.Models;

namespace ContestSystem.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(x =>
                x.Username == dto.Username &&
                x.Password == dto.Password);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = JwtHelper.GenerateToken(user);

        return Ok(new
        {
            token,
            role = user.Role
        });
    }

    [HttpPost("signup")]
    public IActionResult Signup(RegisterDto dto)
    {
    if (_context.Users.Any(u => u.Username == dto.Username))
        return BadRequest("Username already exists");

    var user = new User
    {
        Username = dto.Username,
        Password = dto.Password,
        Role = "Normal"
    };

    _context.Users.Add(user);
    _context.SaveChanges();

    return Ok("User registered successfully");
    }
}