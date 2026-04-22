using HouseRentingSystemApi.Data;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HouseRentingSystemApi.Controllers
{
	[Route("api/[controller]")]
	public class HouseController : ControllerBase
	{
		private AppDbContext context;

		public HouseController(AppDbContext context)
		{
			this.context = context;
		}

		[HttpGet("All")]
		[Produces(typeof(IEnumerable<HouseDetailModel>))]
		public async Task<IActionResult> GetAll()
		{
			var model = await context.Houses
				.AsNoTracking()
				.Where(h => !h.IsDeleted)
				.Select(h => new HouseDetailModel()
				{
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl
				})
				.ToListAsync();

			return Ok(model);
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
			var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);
			if (house == null || house.IsDeleted)
			{
				return NotFound();
			}

			return Ok(new HouseDetailModel()
			{
				Title = house.Title,
				Address = house.Address,
				ImageUrl = house.ImageUrl
			});
		}

		[Authorize]
		[HttpPost("create")]
		[Produces(typeof(HouseDetailModel))]
		public async Task<IActionResult> Create([FromBody]HouseDetailModel model)
		{
			if (ModelState.IsValid == false)
			{
				return BadRequest();
			}
			
			var isAuthenticated = User.Identity?.IsAuthenticated;
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var newHouse = new House()
			{
				Description = model.Description,
				PricePerMonth = model.PricePerMonth,
				Address = model.Address,
				Title=model.Title,

				ImageUrl =model.ImageUrl
			};

			//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userEmail = User.FindFirstValue(ClaimTypes.Email);

			var category = await context.Categories
				.FirstOrDefaultAsync(c => c.Name == model.Category
				.ToString());
			if (category == null) 
			{
				var newCategory = new Category()
				{
					Name = model.Category.ToString(),
				};
				context.Categories.Add(newCategory);
				await context.SaveChangesAsync();
				newHouse.CategoryId = newCategory.Id;
			}
			else
			{
				newHouse.CategoryId = category.Id;
			}
			newHouse.UserId = userId;
			context.Houses.Add(newHouse);
			await context.SaveChangesAsync();

			return Created($"api/All/{newHouse.Id}",new HouseDetailModel() 
			{ 
				Address = newHouse.Address,
				ImageUrl = newHouse.ImageUrl,
				Title = newHouse.Title,
				Description = newHouse.Description,
				PricePerMonth = newHouse.PricePerMonth,
				Category = model.Category
			});
		}

		[Authorize]
		[HttpPut("edit/{id}")]
		[Produces(typeof(HouseDetailModel))]
		public async Task<IActionResult> Edit(int id, [FromBody]HouseDetailModel model)
		{
			if (ModelState.IsValid == false)
			{
				ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(e => Console.WriteLine(e.ErrorMessage));
				return BadRequest(ModelState.Values.SelectMany(v => v.Errors).ToList().First().ErrorMessage);
			}
			
			House? house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id)!;

			if (house == null || house.IsDeleted)
			{
				return NotFound("House not found!");
			}

			Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Name == model.Category.GetDisplayName());

			if(category == null)
			{
				return NotFound("Category not found!");
			}

			house.Category = category;
			house.Title = model.Title;
			house.Address = model.Address;
			house.Description = model.Description;
			house.PricePerMonth = model.PricePerMonth;
			house.ImageUrl = model.ImageUrl;

			await context.SaveChangesAsync();
			return Ok();
		}

		[Authorize]
		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			House? house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id)!;

			if (house == null)
			{
				return NotFound("House not found!");
			}

			if (house.IsDeleted)
			{
				return BadRequest("House already deleted.");
			}

			house.IsDeleted = true;
			context.SaveChanges();
			
			return Ok("House successfully deleted!");
		}
	}
}
