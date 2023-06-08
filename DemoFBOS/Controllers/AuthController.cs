using DemoFBOS.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoFBOS.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly DemofbosContext _context;

		public AuthController(IConfiguration configuration, DemofbosContext context)
		{
			_configuration = configuration;
			_context = context;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel model)
		{
			var user = _context.Users.Include(u => u.Roles).SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
			if (user == null)
				return Unauthorized();

			var token = GenerateJwtToken(user);
			return Ok(new { token });
		}

		[AllowAnonymous]
		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
		{
			var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
			if (existingUser != null)
			{
				return BadRequest("Username already exists.");
			}

			var user = new User { Username = model.Username, Password = model.Password };

			var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
			if (userRole != null)
			{
				user.Roles.Add(userRole);
			}

			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			return Ok();
		}

		private string GenerateJwtToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
			};

			var roles = user.Roles.Select(r => r.Name).ToList();
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));

				var rolePermissions = _context.Roles
					.Include(r => r.Permissions)
					.Single(r => r.Name == role)
					.Permissions
					.Select(p => p.Name)
					.ToList();

				foreach (var permission in rolePermissions)
				{
					claims.Add(new Claim("Permission", permission));
				}
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				_configuration["JwtConfig:Issuer"],
				_configuration["JwtConfig:Audience"],
				claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}


	public class SignUpModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

}
