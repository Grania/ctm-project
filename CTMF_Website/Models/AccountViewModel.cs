using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }
	}

	public class RegisterModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
		[Display(Name = "Nhập lại mật khẩu")]
		public string ConfirmPassword { get; set; }

		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}, {1}, {2}")]
		[Display(Name = "Họ và tên")]
		public string Name { get; set; }

		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class Userinfo
	{
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Display(Name = "Họ và tên")]
		public string Name { get; set; }

		[Display(Name = "Vai trò")]
		public string TypeName { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Số tiền trong tài khoản")]
		public int AmountOfMoney { get; set; }
	}
}