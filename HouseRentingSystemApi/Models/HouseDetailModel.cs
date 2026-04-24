using HouseRentingSystemApi.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

using static HouseRentingSystemApi.Data.DataConstants.DataConstants.House;

namespace HouseRentingSystemApi.Models
{
	public class HouseDetailModel
	{

		[MaxLength(TitleMaxLength)]
		public string Title { get; set; } = string.Empty;

		[MaxLength(AddressMaxLength)]
		public string  Address { get; set; } = string.Empty;
		public string ImageUrl { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal PricePerMonth { get; set; }

		public string Category { get; set; } = string.Empty;
	}
}
