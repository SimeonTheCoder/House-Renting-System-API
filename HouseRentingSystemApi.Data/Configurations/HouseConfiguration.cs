using HouseRentingSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystemApi.Data.Configurations
{
	public class HouseConfiguration : IEntityTypeConfiguration<House>
	{
		public void Configure(EntityTypeBuilder<House> builder)
		{
			builder.HasData(SeedHouses());
		}

		private IEnumerable<House> SeedHouses()
		{
			var houses = new List<House>
{
			new House
				{
					Id=4,
					Title = "Modern Apartment in Sofia Center",
					Address = "15 Vitosha Blvd, Sofia",
					Description = "Bright and modern apartment located in the heart of Sofia, close to restaurants and metro.",
					ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688",
					PricePerMonth = 950,
					CategoryId = 1,
					UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
				},
	new House
	{
		Id=5,
		Title = "Cozy Studio near Park",
		Address = "22 Borisova Gradina, Sofia",
		Description = "Small but cozy studio next to the park, ideal for students or young professionals.",
		ImageUrl = "https://images.unsplash.com/photo-1493809842364-78817add7ffb",
		PricePerMonth = 600,
		CategoryId = 2,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=6,
		Title = "Luxury Penthouse with View",
		Address = "8 Cherni Vrah Blvd, Sofia",
		Description = "Spacious penthouse with panoramic city views and modern interior.",
		ImageUrl = "https://images.unsplash.com/photo-1507089947368-19c1da9775ae",
		PricePerMonth = 1800,
		CategoryId = 3,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=7,
		Title = "Family House in Suburbs",
		Address = "5 Oak Street, Bankya",
		Description = "Quiet family house with garden, perfect for long-term living.",
		ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
		PricePerMonth = 1200,
		CategoryId = 1,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=8,
		Title = "Minimalist Apartment",
		Address = "45 Bulgaria Blvd, Sofia",
		Description = "Clean and minimalist apartment with all necessary amenities.",
		ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267",
		PricePerMonth = 800,
		CategoryId = 2,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=9,
		Title = "Sea View Apartment",
		Address = "10 Coastal Road, Varna",
		Description = "Beautiful apartment with sea view, just minutes from the beach.",
		ImageUrl = "https://images.unsplash.com/photo-1505691938895-1758d7feb511",
		PricePerMonth = 1100,
		CategoryId = 3,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=10,
		Title = "Compact Studio in Plovdiv",
		Address = "12 Kapana District, Plovdiv",
		Description = "Compact studio in the artistic Kapana district.",
		ImageUrl = "https://images.unsplash.com/photo-1499951360447-b19be8fe80f5",
		PricePerMonth = 550,
		CategoryId = 2,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=11,
		Title = "Spacious House with Garden",
		Address = "7 Green Hill, Sofia",
		Description = "Large house with private garden and garage.",
		ImageUrl = "https://images.unsplash.com/photo-1570129477492-45c003edd2be",
		PricePerMonth = 1500,
		CategoryId = 1,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=12,
		Title = "Modern Loft",
		Address = "33 Industrial Zone, Sofia",
		Description = "Stylish loft with open space design and high ceilings.",
		ImageUrl = "https://images.unsplash.com/photo-1484154218962-a197022b5858",
		PricePerMonth = 1000,
		CategoryId = 3,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	},
	new House
	{
		Id=13,
		Title = "Budget Room",
		Address = "18 Studentski Grad, Sofia",
		Description = "Affordable room suitable for students.",
		ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
		PricePerMonth = 400,
		CategoryId = 2,
		UserId = "df26734f-abc3-4aba-b5de-bea3da51c483"
	}
};

			return houses;
		}
	}
}
