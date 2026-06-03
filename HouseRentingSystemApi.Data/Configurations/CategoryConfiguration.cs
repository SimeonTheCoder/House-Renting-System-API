using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystemApi.Data.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new List<Category>()
                {
                    new Category() { Id = 1, Name = "TwoBedroom" },
                    new Category() { Id = 2, Name = "Apartment" },
                    new Category() { Id = 3, Name = "SingleBedroom" },
                }
            );
        }
    }
}
