using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HouseRentingSystemApi.Contracts;
using HouseRentingSystemApi.Data.DataConstants;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HouseRentingSystemApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config
        )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

            this.config = config;
        }

        [HttpPost("/login")]
        [Produces(typeof(AuthResult))]
        public async Task<string> Login([FromBody] AuthModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new ArgumentException("Invalid email or password");

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (result == false)
                throw new ArgumentException("Invalid email or password");

            return await GenerateJwtToken(user);
        }

        public async Task<string> Resgister([FromBody] AuthModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
                throw new ArgumentException("User Already Exists!");

            if (model.Role != UserRoleNames.Client && model.Role != UserRoleNames.Agent)
                throw new ArgumentException("Invalid Role!");

            var newUser = new AppUser() { Email = model.Email, UserName = model.Username };
            var userCreationSuccessful = await userManager.CreateAsync(newUser, model.Password);

            if (!userCreationSuccessful.Succeeded)
            {
                throw new ArgumentException(
                    string.Join(
                        "; ",
                        userCreationSuccessful.Errors.Select(e => e.Description).ToArray()
                    )
                );
            }

            var userRoleAssignmentSuccessful = await userManager.AddToRoleAsync(
                newUser,
                model.Role
            );

            if (!userRoleAssignmentSuccessful.Succeeded)
            {
                throw new ArgumentException(
                    string.Join(
                        "; ",
                        userCreationSuccessful.Errors.Select(e => e.Description).ToArray()
                    )
                );
            }

            return await GenerateJwtToken(newUser);
        }

        public async Task<string> GenerateJwtToken(AppUser user)
        {
            var jwtSection = config.GetSection("Jwt");
            var key = jwtSection["Key"]!;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user))[0]),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiresMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
