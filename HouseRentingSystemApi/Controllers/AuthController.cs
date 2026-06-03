using HouseRentingSystemApi.Contracts;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly IAuthService authService;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            IAuthService authService
        )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.authService = authService;

            this.config = config;
        }

        [HttpPost("/login")]
        [Produces(typeof(AuthResult))]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            if (ModelState.IsValid == false)
            {
                var allErrors = ModelState
                    .Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(PopulateResult(400, null, allErrors));
            }

            try
            {
                return Ok(
                    PopulateResult(
                        200,
                        await authService.Login(model),
                        "User logged in succesfully"
                    )
                );
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/register")]
        [Produces(typeof(AuthResult))]
        public async Task<IActionResult> Resgister([FromBody] AuthModel model)
        {
            if (ModelState.IsValid == false)
            {
                var allErrors = ModelState
                    .Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return Unauthorized(PopulateResult(400, null, allErrors));
            }

            try
            {
                string token = await authService.Resgister(model);
                return Ok(PopulateResult(200, token, "User registered Successfully"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        private AuthResult PopulateResult(int code, string? token = null, params string[] messages)
        {
            var result = new AuthResult()
            {
                Code = code,
                Message = string.Join(Environment.NewLine, messages),
            };

            if (token != null)
                result.Token = token;

            return result;
        }
    }
}
