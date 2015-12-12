using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class TransactionViewModel
	{
	}

	public class RechargeMoney
	{
		[Required(ErrorMessage = "Vui lòng nhập Tên người nộp tiền")]
		[Display(Name = "Tên người nộp tiền")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số tiền")]
		[Range(0, int.MaxValue, ErrorMessage = "Vui lòng điền số dương")]
		[Display(Name = "Số tiền")]
		public int AmountOfMoney { get; set; }
	}

	public class EditTransaction
	{
		[Display (Name = "Mã lịch sử giao dịch")]
		public int TransactionHistoryID { get; set; }

		[Display(Name = "Tên người dùng")]
		public string Username { get; set; }

		[Display(Name = "Loại giao dịch")]
		public int TransactionTypeID { get; set; }

		[Required (ErrorMessage = "Vui lòng nhập số tiền nạp")]
		[Display(Name = "Số tiền nạp")]
		public int Value { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập nội dung giao dịch")]
		[Display(Name = "Nội dung giao dịch")]
		public string TransactionContent { get; set; }

		[Display(Name = "Chi tiết kế hoạch suất ăn")]
		public int? ScheduleMealSetDetailID { get; set; }

		[Required(ErrorMessage = "Vui lòng tích lựa chọn")]
		[Display(Name = "Tự động")]
		public Boolean IsAuto { get; set; }

		[Display(Name = "Ngày giao dịch")]
		public DateTime InsertedDate { get; set; }

		[Display(Name = "Người cập nhật")]
		public string UpdatedBy { get; set; }

		[Display(Name = "Ngày giao dịch")]
		public DateTime LastUpdated { get; set; }
	}

	public class DetailTransaction 
	{
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Display(Name = "Tên người dùng")]
		public string Name { get; set; }

		[Display(Name = "Tên loại giao dịch")]
		public string TransactionTypeName { get; set; }

		[Display(Name = "Nội dung giao dịch")]
		public string TransactionContent { get; set; }

		[Display(Name = "Giá trị giao dịch")]
		public int Value { get; set; }

		[Display(Name = "Thời gian giao dịch")]
		public DateTime InsertedDate { get; set; }

		[Display(Name = "Được cập nhật bởi")]
		public string UpdatedBy { get; set; }

		[Display(Name = "Thời gian cập nhật cuối cùng")]
		public DateTime LastUpdated { get; set; }

		[Display(Name = "Tên suất ăn")]
		public string DishName { get; set; }


		public object TransactionTypeID { get; set; }
	}
}