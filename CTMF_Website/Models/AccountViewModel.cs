using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }
	}

	public class RegisterModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![_.0-9])(?!.*[_.]{2})[a-zA-Z0-9]+(?<![_.])$", ErrorMessage = "Tên đăng nhập sai định dạng")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`])(?=.*[A-z])(?=.*[0-9])(?=.*?[!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`])[a-zA-Z0-9!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+$", ErrorMessage = "Mật khẩu sai định dạng")]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
		[Display(Name = "Nhập lại mật khẩu")]
		public string ConfirmPassword { get; set; }

		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![_. 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`0-9]+(?<![_. 0-9])$", ErrorMessage = "Họ và tên sai định dạng")]
		[Display(Name = "Họ và tên")]
		public string Name { get; set; }

		[EmailAddress(ErrorMessage = "Email Không hợp lệ")]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class UserinfoModel
	{
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[Display(Name = "Họ và tên")]
		public string Name { get; set; }

		[Display(Name = "Loại người dùng")]
		public string TypeName { get; set; }

		[Display(Name = "Vai trò")]
		public int Role { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Số tiền trong tài khoản")]
		public int AmountOfMoney { get; set; }
	}

	public class UserInfoDetailModel
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

		[Display(Name = "Loại người dùng")]
		public string UserTypeID { get; set; }

		[Display(Name = "Vai trò")]
		public int Role { get; set; }
	}

	public class EditUserModel
	{
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[Display(Name = "Tên đăng nhập")]
		public string Username { get; set; }

		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![_. 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`0-9]+(?<![_. 0-9])$", ErrorMessage = "Họ và tên sai định dạng")]
		[Display(Name = "Họ và tên")]
		public string Name { get; set; }

		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Loại người dùng")]
		public string UserTypeID { get; set; }

		[Display(Name = "Vai trò")]
		public int Role { get; set; }

		[Display(Name = "Kích hoạt")]
		public bool isActive { get; set; }
	}

	public class ChangePassword
	{
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ.")]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu cũ")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
		[DataType(DataType.Password)]
		[StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tối đa {1} ký tự, tối thiểu {2} ký tự")]
		[RegularExpression(@"^(?![!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`])(?=.*[A-z])(?=.*[0-9])(?=.*?[!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`])[a-zA-Z0-9!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]+$", ErrorMessage = "Mật khẩu sai định dạng")]
		[Display(Name = "Mật khẩu mới")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu mới không khớp")]
		[Display(Name = "Nhập lại mật khẩu mới")]
		public string ConfirmPassword { get; set; }
	}
}