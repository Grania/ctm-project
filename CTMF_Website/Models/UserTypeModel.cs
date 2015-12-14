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
		[StringLength(3, MinimumLength = 1, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[Display(Name = "Tên viết tắt")]
		public string typeShortName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên loại người dùng")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![_. 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`0-9]+(?<![_. 0-9])$", ErrorMessage = "Tên sai định dạng")]
		[Display(Name = "Tên loại người dùng")]
		public string typeName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số tiền")]
		[Range(0, int.MaxValue, ErrorMessage = "Vui lòng điền số dương")]
		[Display(Name = "Tiền ăn mỗi bữa")]
		public int mealValue { get; set; }

		[Display(Name = "Tiền ăn thêm")]
		[Range(0, int.MaxValue, ErrorMessage = "Vui lòng điền số dương")]
		public int? moreMealValue { get; set; }

		[Display(Name = "Mô tả")]
		public string description { get; set; }

		[Display(Name = "Nợ tín dụng?")]
		public Boolean canDebt { get; set; }

		[Display(Name = "Ăn thêm?")]
		public Boolean canEatMore { get; set; }

		public DateTime insertedDate { get; set; }

		public DateTime lastUpdated { get; set; }

		public string updatedBy { get; set; }
	}
}