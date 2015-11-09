using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class DishViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên món ăn")]
		[Display(Name = "Tên món ăn")]
		public string Dishname { get; set; }

		public string Dishtypeid { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

	}
}