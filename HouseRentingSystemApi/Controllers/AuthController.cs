using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HouseRentingSystemApi.Controllers
{
	[Route("api/[controller]")]
	
	public class AuthController : Controller
	{
		private readonly UserManager<AppUser> userManager;
		private readonly IConfiguration config;

		public AuthController(UserManager<AppUser> userManager
			,IConfiguration config)
		{
			this.userManager = userManager;
			this.config = config;
		}


		[HttpPost("/login")]
		[Produces(typeof(AuthResult))]
		public async Task<IActionResult> Login([FromBody] AuthModel model)
		{
			if (ModelState.IsValid == false)
			{
				var allErrors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToArray();

				return BadRequest(PopulateResult(400, null, allErrors));
			}
			var user =  await userManager.FindByEmailAsync(model.Email);

			if (user == null) 
			{
			
			}

			var result = await userManager.CheckPasswordAsync(user, model.Password);
			if (result == false)
			{
				return Unauthorized(PopulateResult(400, null, "Invalid email or password"));
			}

			var token = GenerateJwtToken(user);
			return Ok(PopulateResult(200,token,"User logged in successfully"));


		}
		[HttpPost("/register")]
		[Produces(typeof(AuthResult))]
		public async Task<IActionResult> Resgister([FromBody]AuthModel model)
		{
			if(ModelState.IsValid == false)
			{
				var allErrors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToArray();

				return Unauthorized(PopulateResult(400,null,allErrors));
			}
			
			var user = await userManager.FindByEmailAsync(model.Email);
			
			if (user != null)
			{
				return BadRequest(PopulateResult(400,null, "User Already exists"));
			}
			var newUser = new AppUser()
			{
				Email = model.Email,
				UserName = model.Username
			};
			var result = await userManager.CreateAsync(newUser,model.Password);

			if (result.Succeeded)
			{
				return Ok(PopulateResult(200,null,"User registered Successfully"));
			}
			
		
			return BadRequest(PopulateResult(
				400,
				null, 
				result.Errors
				.Select(e => e.Description)
				.ToArray()));
		}

		private string GenerateJwtToken(AppUser user)
		{
			var jwtSection = config.GetSection("Jwt");
			var key = jwtSection["Key"]!;

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
				new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName!)
			};

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			var expires = DateTime.UtcNow.AddMinutes(
				int.Parse(jwtSection["ExpiresMinutes"]!)
			);

			var token = new JwtSecurityToken(
				issuer: jwtSection["Issuer"],
				audience: jwtSection["Audience"],
				claims: claims,
				expires: expires,
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private AuthResult PopulateResult(int code,string? token = null, params string[] messages)
		{
			var result = new AuthResult();
			result.Code = code;
			result.Message = string.Join(Environment.NewLine, messages);
			if (token != null)
			{
				result.Token = token;
			}

			return result;
		}
	}
}
