using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace CTMF_Desktop_App.Util
{
	public static class DeviceControl
	{
		//public static List<DeviceModel> deviceList = new List<DeviceModel>();

		public static int newAddr = 1;

		private static readonly uint _addr = 0xffffffff;
		public static readonly byte[] iPwd = { 0 };

		private static readonly string _IMGPath = System.Windows.Forms.Application.StartupPath
			+ "\\ImageTemp\\";

		private static string DisplayToAppAccessString = "I'm CTMF LCD\r";
		private static string AppToDisplayAccessString = "I'm CTMF Client";

		public static readonly string EatMoreCmd = "EatMore\r";

		const string synoDllPath = "SynoAPI.dll";
		[DllImport(synoDllPath)]
		public static extern bool PSOpenDevice(int nDeviceType, int nPortNum, int nPortPara, int nPackageSize = 2);

		[DllImport(synoDllPath)]
		public static extern bool PSCloseDevice();

		[DllImport(synoDllPath)]
		public static extern int PSGetImage(uint nAddr);

		[DllImport(synoDllPath)]
		public static extern int PSVfyPwd(uint nAddr, byte[] iPwd);

		[DllImport(synoDllPath)]
		public static extern int PSUpImage(uint nAddr, [In, Out]byte[] image, ref int len);

		[DllImport(synoDllPath)]
		public static extern int PSImgData2BMP(byte[] image, string file);

		[DllImport(synoDllPath)]
		public static extern int PSSetChipAddr(uint nAddr, byte[] pChipAddr);

		private static uint addAndChangeAddr(uint addr)
		{
			byte[] newByteAdd = BitConverter.GetBytes(newAddr);
			newByteAdd = reverseByteArray(newByteAdd);
			uint nAddr = Convert.ToUInt32(newAddr);

			int re;
			re = PSVfyPwd(addr, iPwd);
			if (re != 0)
			{
				throw new Exception("The input address of the module is wrong.");
			}

			re = PSSetChipAddr(addr, newByteAdd);
			if (re != 0)
			{
				throw new Exception("Error while changing address.");
			}

			re = PSVfyPwd(nAddr, iPwd);
			if (re != 0)
			{
				throw new Exception("New address is wrong.");
			}

			newAddr++;
			return nAddr;
		}

		public static uint connectScanner(int portNum, uint? addr)
		{
			if (!PSOpenDevice(1, portNum, 115200 / 9600))
			{
				throw new Exception("Can't connect module though port COM.");
			}

			if (addr == null)
			{
				int re;
				re = (PSVfyPwd(DeviceControl._addr, iPwd));
				if (re != 0)
				{
					for (uint i = 1; i < 100; i++)
					{
						re = PSVfyPwd(i, iPwd);
						if (re == 0)
						{
							return addAndChangeAddr(i);
						}
					}

					PSCloseDevice();
					throw new Exception("Scanner the address of module fail.");
				}

				return addAndChangeAddr(_addr);
			}
			else
			{
				int re = PSVfyPwd(addr.Value, iPwd);
				if (re != 0)
				{
					re = (PSVfyPwd(_addr, iPwd));
					if (re != 0)
					{
						for (uint i = 1; i < 100; i++)
						{
							re = PSVfyPwd(i, iPwd);
							if (re == 0)
							{
								return addAndChangeAddr(i);
							}
						}

						PSCloseDevice();
						throw new Exception("Scanner the address of module fail.");
					}
				}

				return addAndChangeAddr(addr.Value);
			}
		}

		//return image file path
		public static string getImage(uint addr)
		{
			int re = DeviceControl.PSGetImage(addr);

			if (re != 0 && re != 2)
			{
				throw new Exception("Error while get image.");
			}

			while (re == 2)
			{
				re = DeviceControl.PSGetImage(addr);
			}

			int iLen = 0;
			byte[] image = new byte[256 * 288];
			re = DeviceControl.PSUpImage(addr, image, ref iLen);
			if (re != 0)
			{
				throw new Exception("Error while get image.");
			}

			string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			string file = _IMGPath + fileName + ".bmp";
			re = DeviceControl.PSImgData2BMP(image, file);

			if (re != 0)
			{
				throw new Exception("Error while save image.");
			}

			return file;
		}

		public static string getImage(uint addr, System.ComponentModel.BackgroundWorker bw)
		{
			int re = DeviceControl.PSGetImage(addr);

			if (re != 0 && re != 2)
			{
				throw new Exception("Error while get image.");
			}

			while (re == 2 && !bw.CancellationPending)
			{
				re = DeviceControl.PSGetImage(addr);
			}

			if (bw.CancellationPending)
			{
				return null;
			}

			int iLen = 0;
			byte[] image = new byte[256 * 288];
			re = DeviceControl.PSUpImage(addr, image, ref iLen);
			if (re != 0)
			{
				throw new Exception("Error while get image.");
			}

			string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			string file = _IMGPath + fileName + ".bmp";
			re = DeviceControl.PSImgData2BMP(image, file);

			if (re != 0)
			{
				throw new Exception("Error while save image.");
			}

			return file;
		}

		public static bool connectDisplay(SerialPort serial)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResource.E00001);
			}

			string accessString = "";
			try
			{
				accessString = serial.ReadLine();
			}
			catch (Exception)
			{
				//if Display is idle. Try to close it and reconnect
				serial.WriteLine("close:");

				try
				{
					accessString = serial.ReadLine();
				}
				catch (TimeoutException)
				{
					return false;
				}
			}

			if (accessString == DisplayToAppAccessString)
			{

				serial.WriteLine(AppToDisplayAccessString);
				return true;
			}
			return false;
		}

		public static void setLCDText(SerialPort serial, string text)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResource.E00001);
			}

			string[] splitedStr = text.Split('\n');

			if (splitedStr.Length > 2)
			{
				throw new Exception("Can't write more than 2 line.");
			}
			else
			{
				for (int i = 0; i < splitedStr.Length; i++)
				{
					if (splitedStr[i].Length > 16)
					{
						splitedStr[i] = splitedStr[i].Substring(0, 16);
					}
				}
			}

			serial.WriteLine("lcd:print:" + splitedStr.Length);
			serial.WriteLine(splitedStr[0]);
			if (splitedStr.Length > 1)
			{
				serial.WriteLine(splitedStr[1]);
			}
		}

		// red = 1, green = 2
		public static void setLED(SerialPort serial, int ledId, bool isOn)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResource.E00001);
			}

			serial.WriteLine("led:" + (isOn ? "on" : "off") + ":" + ledId);
		}

		public static void clearLED(SerialPort serial)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResource.E00001);
			}

			serial.WriteLine("led:clear:");
		}

		public static void close(SerialPort serial)
		{
			if (serial.IsOpen)
			{
				clearLED(serial);
				serial.WriteLine("close:");
				serial.Close();
			}
			PSCloseDevice();
		}

		private static byte[] reverseByteArray(byte[] arr)
		{
			for (int i = 0; i < arr.Length / 2; i++)
			{
				byte tmp = arr[i];
				arr[i] = arr[arr.Length - i - 1];
				arr[arr.Length - i - 1] = tmp;
			}

			return arr;
		}
	}
}
