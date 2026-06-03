using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemApi.Contracts
{
    public interface IAuthService
    {
        public Task<string> Login([FromBody] AuthModel model);

        public Task<string> Resgister([FromBody] AuthModel model);

        public Task<string> GenerateJwtToken(AppUser user);
    }
}
