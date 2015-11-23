using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Desktop_App.Util
{
	public static class StringExtensions
	{
		public static bool ContainsInsensitive(string source, string toCheck)
		{
			return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		public static bool EqualsInsensitive(string source, string toCheck)
		{
			if (source.Length != toCheck.Length)
			{
				return false;
			}
			return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}