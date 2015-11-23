using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMF_Desktop_App.Forms
{
	public class ComboBoxItem
	{
		public string Name;
		public string Value;
		public int IntValue;

		public ComboBoxItem(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		public ComboBoxItem(string name, int intValue)
		{
			this.Name = name;
			this.IntValue = intValue;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
