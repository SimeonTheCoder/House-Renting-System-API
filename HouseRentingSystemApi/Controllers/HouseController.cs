using System.Security.Claims;
using HouseRentingSystemApi.Contracts;
using HouseRentingSystemApi.Data;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemApi.Controllers
{
    [Route("api/[controller]")]
    public class HouseController : ControllerBase
    {
        private AppDbContext context;
        private IHouseService houseService;
        private UserManager<AppUser> userManager;
        private const int ItemsPerPage = 4;

        public HouseController(
            AppDbContext context,
            UserManager<AppUser> userManager,
            IHouseService service
        )
        {
            this.context = context;
            this.userManager = userManager;
            this.houseService = service;
        }

        public bool ShouldInclude(House house, string search, string category)
        {
            bool isValidCategory = true;

            if (category.Trim() != "")
                isValidCategory = house.Category.Name.ToLower().Contains(category.ToLower());

            bool isInSearch = true;

            if (search.Trim() != "")
                isInSearch =
                    house.Title.ToLower().Contains(search.ToLower())
                    || house.Description.ToLower().Contains(search.ToLower());

            return isValidCategory && isInSearch;
        }

        [HttpGet("All")]
        [Produces(typeof(IEnumerable<HouseDetailModel>))]
        public async Task<IActionResult> GetAll(
            [FromQuery] string category,
            [FromQuery] string search,
            [FromQuery] string sort,
            [FromQuery] int page
        )
        {
            try
            {
                return Ok(await houseService.GetAll(category, search, sort, page));
            }
            catch (Exception)
            {
                return BadRequest("Invalid query!");
            }
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new { isAuthenticated, userId });
        }

        [HttpGet("{id}")]
        [Produces(typeof(HouseDetailModel))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await houseService.GetById(id));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Agent")]
        [HttpPost("create")]
        [Produces(typeof(HouseDetailModel))]
        public async Task<IActionResult> Create([FromBody] HouseDetailModel model)
        {
            if (ModelState.IsValid == false)
                return BadRequest();

            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
                return Unauthorized();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                House house = await houseService.Create(model, userId!);

                HouseDetailModel dto = new()
                {
                    Address = house.Address,
                    ImageUrl = house.ImageUrl,
                    Title = house.Title,
                    Description = house.Description,
                    PricePerMonth = house.PricePerMonth,
                    Category = model.Category,
                };

                return Created($"api/All/{house.Id}", house);
            }
            catch (NullReferenceException nre)
            {
                return NotFound(nre.Message);
            }
        }

        [Authorize(Roles = "Agent")]
        [HttpPut("edit/{id}")]
        [Produces(typeof(HouseDetailModel))]
        public async Task<IActionResult> Edit(int id, [FromBody] HouseDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState
                    .Values.SelectMany(v => v.Errors)
                    .ToList()
                    .ForEach(e => Console.WriteLine(e.ErrorMessage));

                return BadRequest(
                    ModelState.Values.SelectMany(v => v.Errors).ToList().First().ErrorMessage
                );
            }

            try
            {
                House editedHouse = await houseService.Edit(id, model);

                HouseDetailModel dto = new()
                {
                    Address = editedHouse.Address,
                    ImageUrl = editedHouse.ImageUrl,
                    Title = editedHouse.Title,
                    Description = editedHouse.Description,
                    PricePerMonth = editedHouse.PricePerMonth,
                    Category = model.Category,
                };

                return Ok(dto);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "Agent")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await houseService.Delete(id);
                return Ok("House successfully deleted!");
            }
            catch (NullReferenceException nre)
            {
                return NotFound(nre.Message);
            }
        }

        [Authorize(Roles = "Client")]
        [HttpPost("rent/{id}")]
        public async Task<IActionResult> Rent(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return NotFound("User not found!");

            try
            {
                await houseService.Rent(id, userId);
                return Ok("House successfully rented!");
            }
            catch (NullReferenceException nre)
            {
                return NotFound(nre.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
