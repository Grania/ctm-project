using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Website.Util
{
	public static class StringExtensions
	{
		public static bool ContainsInsensitive(string source, string toCheck)
		{
			return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}