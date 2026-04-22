using System.ComponentModel.DataAnnotations;

using static HouseRentingSystemApi.Data.DataConstants.DataConstants.Category;

namespace HouseRentingSystemApi.Data.Entities
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		public ICollection<House> Hosues { get; set; } = new List<House>();
	}
}
