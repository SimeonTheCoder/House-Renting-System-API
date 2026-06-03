using System.Text;
using HouseRentingSystemApi.Data;
using HouseRentingSystemApi.Data.DataConstants;
using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HouseRentingSystemApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
                options.AddPolicy(
                    "FrontendPolicy",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                )
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            builder
                .Services.AddIdentity<AppUser, IdentityRole>(opt =>
                {
                    opt.SignIn.RequireConfirmedEmail = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            //---NEW SECTION---
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var key = jwtSection["Key"];

            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                    };
                });
            builder.Services.AddAuthorization();
            //--END NEW SECTION--
            var app = builder.Build();

            app.UseStopwatch();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateAsyncScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<
                    RoleManager<IdentityRole>
                >();

                await roleManager.CreateAsync(new IdentityRole(UserRoleNames.Client));

                await scope
                    .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>()
                    .CreateAsync(new IdentityRole(UserRoleNames.Agent));

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                List<AppUser> users = await userManager.Users.ToListAsync();

                foreach (AppUser user in users)
                {
                    bool isClient = await userManager.IsInRoleAsync(user, UserRoleNames.Client);
                    bool isAgent = await userManager.IsInRoleAsync(user, UserRoleNames.Agent);

                    if (isClient || isAgent)
                        continue;

                    await userManager.AddToRoleAsync(user, UserRoleNames.Client);
                }
            }

            app.UseCors("FrontendPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
