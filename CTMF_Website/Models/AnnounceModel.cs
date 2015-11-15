using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Website.Models
{
	public class AnnounceModel
	{
		public int annoucemenID { get; set; }
		public string title { get; set; }
		public string subject { get; set; }
		public Boolean isAuto { get; set; }
		public DateTime insertDate { get; set; }
		public string updateBy { get; set; }
		public DateTime lastUpdate { get; set; }
	}
}