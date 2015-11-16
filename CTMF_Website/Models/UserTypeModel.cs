using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class UserTypeModel
	{
		[Required(ErrorMessage = "Vui lòng nhập  tên viết tắt của phân loại người sử dụng")]
		[StringLength(3, MinimumLength = 1, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên viết tắt của phân loại người sử dụng")]
		public string typeShortName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên phân loại người sử dụng")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Kiểu người sử dụng viết tắt")]
		public string typeName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số tiền mỗi bữa ăn qui định")]
		[Display(Name = "Số tiền ăn mỗi bữa quy định")]
		public int mealValue { get; set; }

		[Display(Name = "Số tiền thêm ăn mỗi bữa cho phép")]
		public int? moreMealValue { get; set; }

		[Display(Name = "Mô tả phân loại người sử dụng")]
		public string description { get; set; }

		[Required(ErrorMessage = "Vui lòng loại người sử dụng có thể sử dụng tín dụng nợ hay không")]
		[Display(Name = "có thể sử dụng tín dụng nợ hay không")]
		public Boolean canDebt { get; set; }

		[Required(ErrorMessage = "Vui lòng loại người sử dụng có thể sử dụng bữa ăn thêm hay không")]
		[Display(Name = "có thể sử dụng bữa ăn thêm hay không")]
		public Boolean canEatMore { get; set; }
		public DateTime insertedDate { get; set; }
		public DateTime lastUpdated { get; set; }
		public string updatedBy { get; set; }

	}
}