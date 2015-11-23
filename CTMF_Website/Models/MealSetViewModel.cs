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
		[Display(Name = "Tên suất ăn")]
		public string MealSetName { get; set; }

		[Display(Name = "Mô tả")]
		public string Description { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Image { get; set; }

		[Display(Name = "Ăn thêm")]
		public bool CanEatMore { get; set; }
	}
}