using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMF_Desktop_App.Util
{
	public class DeviceModel
	{
		private static int _count = 0;

		public SerialPort serial { get; set; }
		public uint scannerAddress { get; set; }
		public bool isForEating { get; set; }
		public string name { get; set; }
		public System.ComponentModel.BackgroundWorker backgroundWokder { get; set; }

		public bool canEatMore { get; set; }
		public bool eatMoreFlag { get; set; }

		public char mealSetLabel { get; set; }
		public int? scheduleMealSetDetailID { get; set; }
		public string mealSetName { get;set; }

		public DeviceModel()
		{

		}

		public DeviceModel(SerialPort serial, uint scannerAddress, bool isForEating = true)
		{
			this.serial = serial;
			this.scannerAddress = scannerAddress;
			this.isForEating = isForEating;
		}

		public bool isValid()
		{
			if (scannerAddress == 0 || (isForEating && (serial == null || String.IsNullOrWhiteSpace(name))))
			{
				return false;
			}
			return true;
		}

		public static string GetName()
		{
			_count++;
			return "Thiết bị số " + _count;
		}

		public static void SetCount(int count)
		{
			_count = count;
		}

		public void Close()
		{
			DeviceControl.close(serial);
		}
	}
}
