using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMF_Desktop_App.Forms.ExtentionClass
{
	[Serializable]
	public class Customer : SourceAFIS.Simple.Person
	{
		public string Username { get; set; }
		public string TypetShortName { get; set; }
		public int MealValue { get; set; }
		public bool CanDebt { get; set; }
		public bool CanEatMore { get; set; }
		public int? MoreMealValue { get; set; }
	}
}
