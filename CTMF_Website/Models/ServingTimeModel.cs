using System;
using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class ServingTimeModel
	{
		public int servingTimeID { set; get; }

		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên ")]
		public string name { set; get; }

		[Required(ErrorMessage="Vui lòng nhập thời gian đăng nhập")]
		[Display(Name="Thời gian bắt đầu")]
		public TimeSpan startTime { set; get; }
		[Required(ErrorMessage = "Vui lòng nhập thời gian đăng nhập")]
		[Display(Name = "Thời gian kết thúc")]
		public TimeSpan? endTime { set; get; }
		public DateTime insertDate { set; get; }
		public DateTime lastUpdate { set; get; }
	}
}