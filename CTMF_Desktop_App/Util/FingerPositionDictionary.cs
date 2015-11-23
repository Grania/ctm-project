using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMF_Desktop_App.Util
{
	public static class FingerPositionDictionary
	{
		private static readonly Dictionary<int, string> fingerPosition = new Dictionary<int, string>()
		{
			{1, "Ngón út trái."},
			{2, "Ngón nhẫn trái"},
			{3, "Ngón giữa trái"},
			{4, "Ngón trỏ trái"},
			{5, "Ngón cái trái"},
			{6, "Ngón cái phải"},
			{7, "Ngón trỏ phải"},
			{8, "Ngón giữa phải"},
			{9, "Ngón nhẫn phải"},
			{10, "Ngón út phải"},
		};

		public static List<KeyValuePair<int, string>> getList()
		{
			return fingerPosition.ToList();
		}

		public static Dictionary<int, string> get()
		{
			return fingerPosition;
		}

		public static string getFingerName(int position)
		{
			if (position <= 0 || position > 10)
			{
				return null;
			}

			return fingerPosition.Where(f => f.Key == position).Single().Value;
		}
	}
}
