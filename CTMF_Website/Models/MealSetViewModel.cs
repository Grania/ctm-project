using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class MealSetViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên suất ăn")]
		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} nhiều nhất {1} ký tự, ít nhất {2}")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+(?<![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` ])$", ErrorMessage = "Tên suất ăn sai định dạng")]
		[Display(Name = "Tên suất ăn")]
		public string MealSetName { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }

		[Display(Name = "Ăn thêm")]
		public bool CanEatMore { get; set; }
	}

	public class EditMealSetModel
	{
		public int MealSetID { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên suất ăn")]
		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} nhiều nhất {1} ký tự, ít nhất {2}")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+(?<![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|` ])$", ErrorMessage = "Tên suất ăn sai định dạng")]
		[Display(Name = "Tên suất ăn")]
		public string MealSetName { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }

		[Display(Name = "Ăn thêm")]
		public bool CanEatMore { get; set; }
	}

	public class MealSetDishModel
	{
		public int MealSetID { get; set; }

		public int DishID { get; set; }

		public string Dishname { get; set; }

		public int DishTypeID { get; set; }

		public string DishDescription { get; set; }

		public string DishImage { get; set; }
	}
}