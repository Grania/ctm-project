using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class DishViewModel
	{
		public string Dishname { get; set; }
		public string Dishtypeid { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public DateTime Inserteddate { get; set; }
		public string Updatedby { get; set; }
		public DateTime Lastupdated { get; set; }

	}
}