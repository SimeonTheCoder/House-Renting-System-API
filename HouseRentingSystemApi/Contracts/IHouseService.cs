using HouseRentingSystemApi.Data.Entities;
using HouseRentingSystemApi.Models;

namespace HouseRentingSystemApi.Contracts
{
    public interface IHouseService
    {
        public Task<List<HouseDetailModel>> GetAll(
            string category,
            string search,
            string sort,
            int page
        );

        public Task<HouseDetailModel> GetById(int id);

        public Task<House> Create(HouseDetailModel model, string userId);

        public Task<House> Edit(int id, HouseDetailModel model);

        public Task<House> Delete(int id);

        public Task<House> Rent(int id, string userId);
    }
}
