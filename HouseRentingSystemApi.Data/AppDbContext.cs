using HouseRentingSystemApi.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HouseRentingSystemApi.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext()
		{
			
		}
		public AppDbContext(DbContextOptions<AppDbContext> options)
			:base(options) 
		{
			
		}

	
		public DbSet<House> Houses { get; set; }
		public DbSet<Category> Categories { get; set; }


		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	if (optionsBuilder.IsConfigured == false)
		//	{
		//		optionsBuilder.UseSqlServer();
		//	}
		//}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}
	}
}
