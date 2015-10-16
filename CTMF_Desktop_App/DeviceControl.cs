using System;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace CTMF_Desktop_App
{
	public static class DeviceControl
	{
		const uint addr = 0xffffffff;
		static byte[] iPwd = { 0 };

		static string DisplayToAppAccessString = "I'm CTM LCD\r";
		static string AppToDisplayAccessString = "I'm CTM Client";

		const string synoDllPath = "SynoAPI.dll";
		[DllImport(synoDllPath)]
		static extern bool PSOpenDevice(int nDeviceType, int nPortNum, int nPortPara, int nPackageSize = 2);

		[DllImport(synoDllPath)]
		static extern bool PSCloseDevice();

		[DllImport(synoDllPath)]
		static extern int PSGetImage(uint nAddr);

		[DllImport(synoDllPath)]
		static extern int PSVfyPwd(uint nAddr, byte[] iPwd);

		[DllImport(synoDllPath)]
		static extern int PSUpImage(uint nAddr, [In, Out]byte[] image, ref int len);

		[DllImport(synoDllPath)]
		static extern int PSImgData2BMP(byte[] image, string file);

		public static bool connectDisplay(SerialPort serial)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResources.E00001);
			}

			string accessString = serial.ReadLine();

			if (accessString == DisplayToAppAccessString)
			{

				serial.WriteLine(AppToDisplayAccessString);
				return true;
			}
			return false;
		}

		public static bool connectScanner(int portNum)
		{
			if (PSOpenDevice(1, portNum, 115200 / 9600))
			{
				int re = (PSVfyPwd(addr, iPwd));
				if (re != 0)
				{
					PSCloseDevice();
					return false;
				}
			}
			return true;
		}

		public static void setLCDText(SerialPort serial, string text)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResources.E00001);
			}

			string[] splitedStr = text.Split('\n');
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
				throw new Exception(StringResources.E00001);
			}

			serial.WriteLine("led:" + (isOn ? "on" : "off") + ":" + ledId);
		}

		public static void clearLED(SerialPort serial)
		{
			if (serial == null || !serial.IsOpen)
			{
				throw new Exception(StringResources.E00001);
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
	}
}
