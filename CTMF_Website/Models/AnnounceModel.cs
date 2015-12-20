using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class AnnounceModel
	{
		public int annoucemenID { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập tên thông báo")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[Display(Name = "Tên thông tin ")]
		public string title { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập nội dung thông báo")]
		[Display(Name = "Nội dung tin tức ")]
		public string subject { get; set; }

		[Display(Name = "Thông báo tự động")]
		public Boolean isAuto { get; set; }
	}
}