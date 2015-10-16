using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMF_Desktop_App
{
	public class Device
	{
		public SerialPort serial { get; set; }
		public uint scannerAddress { get; set; }

		public Device(SerialPort serial, uint scannerAddress)
		{
			this.serial = serial;
			this.scannerAddress = scannerAddress;
		}
	}
}
