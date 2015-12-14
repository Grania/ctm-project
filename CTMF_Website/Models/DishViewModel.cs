using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class DishViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên món ăn")]
		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} nhiều nhất {1} ký tự, ít nhất {2}")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+(?<![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` ])$", ErrorMessage = "Tên suất ăn sai định dạng")]
		[Display(Name = "Tên món ăn")]
		public string Dishname { get; set; }

		[Display(Name = "Loại món ăn")]
		public int DishTypeID { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }
	}

	public class EditDishModel
	{
		public int DishID { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên món ăn")]
		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} nhiều nhất {1} ký tự, ít nhất {2}")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+(?<![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` ])$", ErrorMessage = "Tên suất ăn sai định dạng")]
		[Display(Name = "Tên món ăn")]
		public string Dishname { get; set; }

		[Display(Name = "Loại món ăn")]
		public int DishTypeID { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }
	}

	public class DetailDishModel
	{
		public int DishID { get; set; }

		[Display(Name = "Tên món ăn")]
		public string Dishname { get; set; }

		[Display(Name = "Loại món ăn")]
		public string DishTypeName { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }
	}
}