using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class LoginViewModel
	{
		[Required]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }
	}
}