using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class UserTypeModel
	{
		[Required(ErrorMessage = "Vui lòng nhập  tên viết tắt")]
		[StringLength(3, MinimumLength = 1, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên viết tắt")]
		public string typeShortName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên loại người dùng")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên loại người dùng")]
		public string typeName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số tiền")]
		[Display(Name = "Tiền ăn mỗi bữa")]
		public int mealValue { get; set; }

		[Display(Name = "Tiền ăn thêm")]
		public int? moreMealValue { get; set; }

		[Display(Name = "Mô tả")]
		public string description { get; set; }

		[Required(ErrorMessage = "error")]
		[Display(Name = "Nợ tín dụng?")]
		public Boolean canDebt { get; set; }

		[Required(ErrorMessage = "error")]
		[Display(Name = "Ăn thêm?")]
		public Boolean canEatMore { get; set; }
		public DateTime insertedDate { get; set; }
		public DateTime lastUpdated { get; set; }
		public string updatedBy { get; set; }

	}
}