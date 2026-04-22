using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystemApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededHouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "TwoBedroom" },
                    { 2, "Apartment" },
                    { 3, "SingleBedroom" }
                });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "Title", "UserId" },
                values: new object[,]
                {
                    { 4, "15 Vitosha Blvd, Sofia", 1, "Bright and modern apartment located in the heart of Sofia, close to restaurants and metro.", "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688", 950m, "Modern Apartment in Sofia Center", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 5, "22 Borisova Gradina, Sofia", 2, "Small but cozy studio next to the park, ideal for students or young professionals.", "https://images.unsplash.com/photo-1493809842364-78817add7ffb", 600m, "Cozy Studio near Park", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 6, "8 Cherni Vrah Blvd, Sofia", 3, "Spacious penthouse with panoramic city views and modern interior.", "https://images.unsplash.com/photo-1507089947368-19c1da9775ae", 1800m, "Luxury Penthouse with View", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 7, "5 Oak Street, Bankya", 1, "Quiet family house with garden, perfect for long-term living.", "https://images.unsplash.com/photo-1568605114967-8130f3a36994", 1200m, "Family House in Suburbs", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 8, "45 Bulgaria Blvd, Sofia", 2, "Clean and minimalist apartment with all necessary amenities.", "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267", 800m, "Minimalist Apartment", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 9, "10 Coastal Road, Varna", 3, "Beautiful apartment with sea view, just minutes from the beach.", "https://images.unsplash.com/photo-1505691938895-1758d7feb511", 1100m, "Sea View Apartment", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 10, "12 Kapana District, Plovdiv", 2, "Compact studio in the artistic Kapana district.", "https://images.unsplash.com/photo-1499951360447-b19be8fe80f5", 550m, "Compact Studio in Plovdiv", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 11, "7 Green Hill, Sofia", 1, "Large house with private garden and garage.", "https://images.unsplash.com/photo-1570129477492-45c003edd2be", 1500m, "Spacious House with Garden", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 12, "33 Industrial Zone, Sofia", 3, "Stylish loft with open space design and high ceilings.", "https://images.unsplash.com/photo-1484154218962-a197022b5858", 1000m, "Modern Loft", "df26734f-abc3-4aba-b5de-bea3da51c483" },
                    { 13, "18 Studentski Grad, Sofia", 2, "Affordable room suitable for students.", "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85", 400m, "Budget Room", "df26734f-abc3-4aba-b5de-bea3da51c483" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
