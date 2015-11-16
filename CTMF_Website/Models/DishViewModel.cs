using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class DishViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên món ăn")]
		[Display(Name = "Tên món ăn")]
		public string Dishname { get; set; }

		public string Dishtypeid { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }

	}
}