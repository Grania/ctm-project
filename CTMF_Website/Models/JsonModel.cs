using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class ScheduleJsonModel
	{
		public int? MealSetID { get; set; }

		public int? ScheduleMealSetDetailID { get; set; }

		public string Name { get; set; }

		public string Image { get; set; }

		public string Description { get; set; }

		public int ServingTimeID { get; set; }

		public string ServingTimeName { get; set; }

		public bool IsDayOn { get; set; }
	}

	public class ServingTimeJsonModel
	{
		public int ServingTimeID { get; set; }

		public string Name { get; set; }

		public string StartTimeStr { get; set; }

		public string EndTimeStr { get; set; }
	}

	public class MeatSetForDDLJsonModel
	{
		public int MealSetID { get; set; }

		public string Name { get; set; }
	}

	public class UserEatJsonModel
	{
		public int? ScheduleMealSetDetailID { get; set; }
	}
}