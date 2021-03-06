﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CTMF_Website.Models
{
	public class ServingTimeModel
	{
		public int servingTimeID { set; get; }

		[Required(ErrorMessage = "Vui lòng nhập tên thời gian phục vụ")]
		[StringLength(30, MinimumLength = 6, ErrorMessage = "{0} nhiều nhất {1} ký tự, ít nhất {2}")]
		[RegularExpression(@"^(?![_. 0-9])[^!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`0-9]+(?<![_. 0-9])$", ErrorMessage = "Tên sai định dạng")]
		[Display(Name = "Tên ")]
		public string Name { set; get; }

		[Required(ErrorMessage="Vui lòng nhập thời gian bắt đầu")]
		[Display(Name="Thời gian bắt đầu")]
		public TimeSpan startTime { set; get; }

		[Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc")]
		[Display(Name = "Thời gian kết thúc")]
		public TimeSpan endTime { set; get; }

		public DateTime insertDate { set; get; }
		public DateTime lastUpdate { set; get; }
	}
}