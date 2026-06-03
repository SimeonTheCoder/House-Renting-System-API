using HouseRentingSystemApi.Contracts;
using HouseRentingSystemApi.Data;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystemApi.Services
{
    public class HouseService : IHouseService
    {
        private AppDbContext context;
        private UserManager<AppUser> userManager;
        private const int ItemsPerPage = 4;

        public HouseService(AppDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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

        public async Task<List<HouseDetailModel>> GetAll(
            string category,
            string search,
            string sort,
            int page
        )
        {
            var query = context.Houses.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(h =>
                    h.Title.ToLower().Contains(search.ToLower())
                    || h.Description.ToLower().Contains(search.ToLower())
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                if (
                    (
                        await context
                            .Categories.AsNoTracking()
                            .Where(c => c.Name.ToLower() == category)
                            .ToListAsync()
                    ).Count != 1
                )
                    throw new ArgumentException();

                query = query.Where(h => h.Category.Name.ToLower() == category.ToLower());
            }

            var queryB = query.Select(h => new HouseDetailModel()
            {
                Title = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                Category = h.Category.Name,
            });

            queryB =
                (!string.IsNullOrWhiteSpace(sort) && sort == "desc")
                    ? queryB.OrderByDescending(h => h.PricePerMonth)
                    : queryB.OrderBy(h => h.PricePerMonth);

            int allCount = await queryB.CountAsync();

            page--;

            if (page == -1)
            {
                throw new ArgumentException("Invalid page");
            }

            var model = await queryB.Skip(page * ItemsPerPage).Take(ItemsPerPage).ToListAsync();
            return model;
        }

        public async Task<HouseDetailModel> GetById(int id)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);

            if (house == null || house.IsDeleted)
                throw new NullReferenceException("House not found!");

            return new HouseDetailModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
            };
        }

        public async Task<House> Create(HouseDetailModel model, string userId)
        {
            AppUser user = (await userManager.FindByIdAsync(userId))!;

            if (user == null)
            {
                throw new NullReferenceException("User not found!");
            }

            var newHouse = new House()
            {
                Description = model.Description,
                PricePerMonth = model.PricePerMonth,
                Address = model.Address,
                Title = model.Title,
                Owner = user,
                ImageUrl = model.ImageUrl,
            };

            var category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Name == model.Category.ToString()
            );

            if (category == null)
            {
                var newCategory = new Category() { Name = model.Category.ToString() };

                context.Categories.Add(newCategory);
                await context.SaveChangesAsync();

                newHouse.CategoryId = newCategory.Id;
            }
            else
            {
                newHouse.CategoryId = category.Id;
            }

            context.Houses.Add(newHouse);
            await context.SaveChangesAsync();

            return newHouse;
        }

        public async Task<House> Edit(int id, HouseDetailModel model)
        {
            House? house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id)!;

            if (house == null || house.IsDeleted)
            {
                throw new NullReferenceException("House not found!");
            }

            Category? category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Name == model.Category
            );

            if (category == null)
            {
                throw new NullReferenceException("Category not found!");
            }

            house.Category = category;
            house.Title = model.Title;
            house.Address = model.Address;
            house.Description = model.Description;
            house.PricePerMonth = model.PricePerMonth;
            house.ImageUrl = model.ImageUrl;

            await context.SaveChangesAsync();

            return house;
        }

        public async Task<House> Delete(int id)
        {
            House? house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id)!;

            if (house == null || house.IsDeleted)
                throw new NullReferenceException("House not found!");

            house.IsDeleted = true;
            context.SaveChanges();

            return house;
        }

        public async Task<House> Rent(int id, string userId)
        {
            AppUser? user = await userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NullReferenceException("User not found!");

            House? house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);

            if (house == null)
                throw new NullReferenceException("House not found!");

            if (house.Renter != null)
                throw new Exception("House alread rented!");

            house.Renter = user;

            await context.SaveChangesAsync();
            return house;
        }
    }
}
