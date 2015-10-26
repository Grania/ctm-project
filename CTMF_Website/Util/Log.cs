using System;
using System.IO;
using System.Text;

namespace CTMF_Website.Util
{
	public static class Log
	{
		private static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "log\\";

		public static void ErrorLog(string message)
		{
			StreamWriter sw = new StreamWriter(path + getFileName("ErrorLog"), true);
			sw.WriteLine(formatString(message));
			sw.Flush();
			sw.Close();
		}

		public static void ActivityLog(string message)
		{
			StreamWriter sw = new StreamWriter(path + getFileName("ActivityLog"), true);
			sw.WriteLine(formatString(message));
			sw.Flush();
			sw.Close();
		}

		private static string formatString(string message)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));
			sb.Append(": ");
			sb.Append(message);
			return sb.ToString();
		}

		private static string getFileName(string fileName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(fileName);
			sb.Append(DateTime.Now.ToString("ddMMyyyy"));
			sb.Append(".log");
			return sb.ToString();
		}
	}
}
