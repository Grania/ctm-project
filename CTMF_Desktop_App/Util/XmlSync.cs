using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Xml;
using CTMF_Desktop_App.DataAccessTableAdapters;
using System.Data.SqlClient;
using System.IO;

namespace CTMF_Desktop_App.Util
{
	public static class XmlSync
	{
		private static string _path = "Xml/";
		private static string _xmlConfigPath = _path + "config.xml";
		private static string _URLAuthString = "4d740f80a99f32e2eead4727282f22ba";

		internal static string GetSyncID()
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);

			XAttribute syncAttr = xDoc.Element("SyncServer").Attribute("SyncID");
			if (syncAttr == null)
			{
				return null;
			}
			return syncAttr.Value;
		}

		internal static DateTime GetLastSync()
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);

			XAttribute lastSyncAttr = xDoc.Element("SyncServer").Attribute("LastSync");
			if (lastSyncAttr == null)
			{
				throw new Exception("Cannot get LastSync value from config.xml");
			}

			string lastSyncStr = lastSyncAttr.Value;
			DateTime lastSync;
			if (!DateTime.TryParse(lastSyncStr, out lastSync))
			{
				throw new Exception("Cannot parse LastSync value from config.xml");
			}

			return lastSync;
		}

		internal static void SetLastSync(DateTime lastDate)
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);

			XElement syncServerEl = xDoc.Element("SyncServer");
			XAttribute lastSyncAttr = syncServerEl.Attribute("LastSync");

			if (lastSyncAttr == null)
			{
				syncServerEl.Add(new XAttribute("LastSync", lastDate));
			}
			else
			{
				lastSyncAttr.Value = lastDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
			}

			xDoc.Save(_xmlConfigPath);
		}

		internal static void SaveNewSync(string syncID)
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);
			XElement syncServerEl = xDoc.Element("SyncServer");

			if (syncServerEl.Attribute("SyncID") == null)
			{
				syncServerEl.Add(new XAttribute("SyncID", syncID));
			}
			else
			{
				syncServerEl.Attribute("SyncID").Value = syncID;
			}

			if (syncServerEl.Attribute("LastSync") == null)
			{
				syncServerEl.Add(new XAttribute("LastSync", new DateTime().ToString("yyyy-MM-dd HH:mm:ss.fff")));
			}
			else
			{
				syncServerEl.Attribute("LastSync").Value = new DateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
			}

			string fileName = syncID + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";

			XDocument xmlDataDoc = new XDocument();
			xmlDataDoc.Add(new XElement("NewDataSet"));
			xmlDataDoc.Save(_path + fileName);

			syncServerEl.Descendants("XmlData").Remove();
			XElement xmlDataEl = new XElement("XmlData");
			xmlDataEl.Add(new XAttribute("Active", 1));
			xmlDataEl.Add(new XAttribute("Current", 1));
			xmlDataEl.Value = fileName;
			syncServerEl.Add(xmlDataEl);

			xDoc.Save(_xmlConfigPath);
		}

		internal static string RequestXmlFileName(string syncID, bool isExcludeCurrent)
		{
			if (syncID == null || syncID == String.Empty)
			{
				return null;
			}

			ServiceReference.WebServiceSoapClient soapClient = new ServiceReference.WebServiceSoapClient();
			return soapClient.RequestXmlFileName(WebServiceAuth.AuthSoapHeader(), syncID, isExcludeCurrent);
		}

		internal static string RequestSync(DateTime syncDate)
		{
			string syncFilenames = SendXml(syncDate);

			ServiceReference.WebServiceSoapClient soapClient = new ServiceReference.WebServiceSoapClient();
			return soapClient.RequestSync(WebServiceAuth.AuthSoapHeader(), GetSyncID(), syncFilenames);
		}

		internal static bool DeleteSync(string syncID)
		{
			bool isDeleted = false;
			try
			{
				ServiceReference.WebServiceSoapClient soapClient = new ServiceReference.WebServiceSoapClient();
				isDeleted = soapClient.DeleteSync(WebServiceAuth.AuthSoapHeader(), syncID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				throw ex;
			}

			return isDeleted;
		}

		internal static IList<string> SaveXmlFile(string fileNameList)
		{
			string[] fileNameArr = fileNameList.Split('|');
			IList<string> listXmlPath = new List<string>();

			using (var client = new WebClient())
			{
				foreach (string fileName in fileNameArr)
				{
					if (fileName == String.Empty)
					{
						continue;
					}

					string url = ConfigurationManager.AppSettings["ServerURL"] + "DownloadXMLFile.ashx?Auth="
						+ _URLAuthString + "&FileName=" + fileName;
					client.DownloadFile(url, _path + fileName);
					listXmlPath.Add(_path + fileName);
				}
			}

			return listXmlPath;
		}

		internal static void Sync(IList<string> xmlPathList)
		{
			foreach (string xmlPath in xmlPathList)
			{
				Sync(xmlPath);
			}
		}

		internal static void SaveUserInfoXml(string username, string name, string typeShortName, int amountOfMoney
			, DateTime lastUpdatedMoney, byte[] fingerPrintIMG, DateTime? lastUpdatedFingerPrint, int? fingerPosition
			, bool isCafeteriaStaff, bool isActive, DateTime insertedDate, string updatedBy, DateTime lastUpdated)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_xmlConfigPath);

				string xmlFileName = xDoc.Element("SyncServer").Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1")
					.Single().Value;
				xmlFileName = _path + xmlFileName;

				XElement userInfo = new XElement("UserInfo");

				// not null value save
				userInfo.Add(new XElement("Username", username));
				userInfo.Add(new XElement("AmountOfMoney", amountOfMoney));
				userInfo.Add(new XElement("LastUpdatedMoney", lastUpdatedMoney));
				userInfo.Add(new XElement("IsCafeteriaStaff", isCafeteriaStaff));
				userInfo.Add(new XElement("IsActive", isActive));
				userInfo.Add(new XElement("InsertedDate", insertedDate));
				userInfo.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (name != null)
				{
					userInfo.Add(new XElement("Name", name));
				}
				if (typeShortName != null)
				{
					userInfo.Add(new XElement("TypeShortName", typeShortName));
				}
				if (fingerPrintIMG != null)
				{
					userInfo.Add(new XElement("FingerPrintIMG", Convert.ToBase64String(fingerPrintIMG)));
				}
				if (lastUpdatedFingerPrint != null)
				{
					userInfo.Add(new XElement("LastUpdatedFingerPrint", lastUpdatedFingerPrint));
				}
				if (fingerPosition != null)
				{
					userInfo.Add(new XElement("FingerPosition", fingerPosition));
				}
				if (updatedBy != null)
				{
					userInfo.Add(new XElement("UpdatedBy", updatedBy));
				}

				XDocument dataSetXDoc = XDocument.Load(xmlFileName);
				XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

				dataSetEl.Descendants("UserInfo").Where(d => d.Element("Username").Value == typeShortName).Remove();

				dataSetEl.Add(userInfo);

				dataSetXDoc.Save(xmlFileName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveTransactionHistoryXml(int transactionHistoryID, string username, int transactionTypeID
			, int value, string transactionContent, int? scheduleMealSetDetailID, bool isAuto, DateTime insertedDate
			, string updatedBy, DateTime lastUpdated)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_xmlConfigPath);

				string xmlFileName = xDoc.Element("SyncServer").Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1")
					.Single().Value;
				xmlFileName = _path + xmlFileName;

				XElement transactionHistory = new XElement("TransactionHistory");

				// not null value save
				transactionHistory.Add(new XElement("TransactionHistoryID", transactionHistoryID));
				transactionHistory.Add(new XElement("Username", username));
				transactionHistory.Add(new XElement("TransactionTypeID", transactionTypeID));
				transactionHistory.Add(new XElement("Value", value));
				transactionHistory.Add(new XElement("TransactionContent", transactionContent));
				transactionHistory.Add(new XElement("IsAuto", isAuto));
				transactionHistory.Add(new XElement("InsertedDate", insertedDate));
				transactionHistory.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (scheduleMealSetDetailID != null)
				{
					transactionHistory.Add(new XElement("ScheduleMealSetDetailID", scheduleMealSetDetailID));
				}
				if (updatedBy != null)
				{
					transactionHistory.Add(new XElement("UpdatedBy", updatedBy));
				}

				XDocument dataSetXDoc = XDocument.Load(xmlFileName);
				XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

				dataSetEl.Descendants("TransactionHistory").Where(d => d.Element("TransactionHistoryID").Value == transactionHistoryID.ToString()).Remove();

				dataSetEl.Add(transactionHistory);

				dataSetXDoc.Save(xmlFileName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static string SendXml(DateTime syncDate)
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);
			XElement syncServerEl = xDoc.Element("SyncServer");

			string syncID = syncServerEl.Attribute("SyncID").Value;

			syncServerEl.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").Single().Attribute("Current").Value = "0";

			string newFilename = syncID + "-" + syncDate.ToString("yyyyMMddHHmmssfff") + ".xml";
			XDocument xmlDataDoc = new XDocument();
			xmlDataDoc.Add(new XElement("NewDataSet"));
			xmlDataDoc.Save(_path + newFilename);

			XElement newEl = new XElement("XmlData");
			newEl.Value = newFilename;
			newEl.Add(new XAttribute("Active", 1));
			newEl.Add(new XAttribute("Current", 1));
			syncServerEl.Add(newEl);

			xDoc.Save(_xmlConfigPath);

			List<XElement> activeList = syncServerEl.Descendants("XmlData")
				.Where(x => x.Attribute("Active").Value == "1" && x.Attribute("Current").Value == "0").ToList();

			StringBuilder sbFilenames = new StringBuilder();
			foreach (XElement el in activeList)
			{
				string sendingFilename = el.Value;
				string uri = ConfigurationManager.AppSettings["ServerURL"] + "/api/XML/" + sendingFilename.Replace(".xml", "");
				SendXMLFile(sendingFilename, uri, 500);

				sbFilenames.Append(sendingFilename);
				sbFilenames.Append("|");
			}

			return sbFilenames.ToString();
		}

		private static void Sync(string xmlPath)
		{
			XDocument xDoc;
			XElement newDataSetEl;
			DateTime lastSync;
			try
			{
				xDoc = XDocument.Load(xmlPath);
				newDataSetEl = xDoc.Element("NewDataSet");

				lastSync = GetLastSync();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			UserTypeTableAdapter userTypeTA = new UserTypeTableAdapter();
			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			MealSetTableAdapter mealSetTA = new MealSetTableAdapter();
			ServingTimeTableAdapter servingTimeTA = new ServingTimeTableAdapter();
			ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();
			ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();
			TransactionTypeTableAdapter transactionTypeTA = new TransactionTypeTableAdapter();
			TransactionHistoryTableAdapter transactionHistoryTA = new TransactionHistoryTableAdapter();

			userTypeTA.Connection.Open();
			userInfoTA.Connection = userTypeTA.Connection;
			mealSetTA.Connection = userTypeTA.Connection;
			servingTimeTA.Connection = userTypeTA.Connection;
			scheduleTA.Connection = userTypeTA.Connection;
			scheduleMealSetDetailTA.Connection = userTypeTA.Connection;
			transactionTypeTA.Connection = userTypeTA.Connection;
			transactionHistoryTA.Connection = userTypeTA.Connection;

			using (SqlTransaction transaction = userTypeTA.Connection.BeginTransaction())
			{
				userTypeTA.AttachTransaction(transaction);
				userInfoTA.AttachTransaction(transaction);
				mealSetTA.AttachTransaction(transaction);
				servingTimeTA.AttachTransaction(transaction);
				scheduleTA.AttachTransaction(transaction);
				scheduleMealSetDetailTA.AttachTransaction(transaction);
				transactionTypeTA.AttachTransaction(transaction);
				transactionHistoryTA.AttachTransaction(transaction);

				try
				{
					// sync usertype table
					// NOTE: table UserType in subDB only get value from db in web app.
					// so in the desktop app we only insert and update
					IList<XElement> ElList = newDataSetEl.Descendants("UserType").ToList();

					foreach (XElement userTypeEl in ElList)
					{
						// not null value parse
						string typeShortName = userTypeEl.Element("TypeShortName").Value;
						string typeName = userTypeEl.Element("TypeName").Value;
						int mealValue = int.Parse(userTypeEl.Element("MealValue").Value);
						bool canEatMore = bool.Parse(userTypeEl.Element("CanEatMore").Value);
						DateTime insertedDate = DateTime.Parse(userTypeEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(userTypeEl.Element("LastUpdated").Value);
						bool canDebt = bool.Parse(userTypeEl.Element("CanDebt").Value);

						// nullable value parse
						int? moreMealValue;
						if (userTypeEl.Element("MoreMealValue") == null)
						{
							moreMealValue = null;
						}
						else
						{
							moreMealValue = int.Parse(userTypeEl.Element("MoreMealValue").Value.ToString());
						}

						string description;
						if (userTypeEl.Element("Description") == null)
						{
							description = null;
						}
						else
						{
							description = userTypeEl.Element("Description").Value;
						}

						string updatedBy;
						if (userTypeEl.Element("UpdatedBy") == null)
						{
							updatedBy = null;
						}
						else
						{
							updatedBy = userTypeEl.Element("UpdatedBy").Value;
						}

						if (insertedDate > lastSync)
						{
							if ((int)userTypeTA.CheckDuplicateKey(typeShortName) == 0)
							{
								userTypeTA.Insert(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertedDate, updatedBy, lastUpdated);
								continue;
							}
						}

						userTypeTA.Update(typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertedDate, updatedBy, lastUpdated, typeShortName);
					}

					// sync userinfo table
					ElList = newDataSetEl.Descendants("UserInfo").ToList();

					foreach (XElement userInfoEl in ElList)
					{
						// not null value parse
						string username = userInfoEl.Element("Username").Value;
						int amountOfMoney = int.Parse(userInfoEl.Element("AmountOfMoney").Value);
						DateTime lastUpdatedMoney = DateTime.Parse(userInfoEl.Element("LastUpdatedMoney").Value);
						bool isCafeteriaStaff = bool.Parse(userInfoEl.Element("IsCafeteriaStaff").Value);
						bool isActive = bool.Parse(userInfoEl.Element("IsActive").Value);
						DateTime insertedDate = DateTime.Parse(userInfoEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(userInfoEl.Element("LastUpdated").Value);

						// nullable value parse
						string name = userInfoEl.TryGetElementValue("Name");
						string typeShortName = userInfoEl.TryGetElementValue("TypeShortName");
						string updatedBy = userInfoEl.TryGetElementValue("UpdatedBy");

						byte[] fingerPrintIMG;
						if (userInfoEl.Element("FingerPrintIMG") == null)
						{
							fingerPrintIMG = null;
						}
						else
						{
							fingerPrintIMG = Convert.FromBase64String(userInfoEl.Element("FingerPrintIMG").Value);
						}

						DateTime? lastUpdatedFingerPrint;
						if (userInfoEl.Element("LastUpdatedFingerPrint") == null)
						{
							lastUpdatedFingerPrint = null;
						}
						else
						{
							lastUpdatedFingerPrint = DateTime.Parse(userInfoEl.Element("LastUpdatedFingerPrint").Value);
						}

						int? fingerPosition;
						if (userInfoEl.Element("FingerPosition") == null)
						{
							fingerPosition = null;
						}
						else
						{
							fingerPosition = int.Parse(userInfoEl.Element("FingerPosition").Value);
						}

						if (insertedDate > lastSync)
						{
							if ((int)userInfoTA.CheckDuplicateKey(username) == 0)
							{
								userInfoTA.Insert(username, name, typeShortName, amountOfMoney, lastUpdatedMoney, fingerPrintIMG
									, lastUpdatedFingerPrint, fingerPosition, isCafeteriaStaff, isActive, insertedDate
									, updatedBy, lastUpdated);
								continue;
							}
						}

						DataTable userInfoDT = userInfoTA.GetUserInfo(username);
						if (userInfoDT.Rows.Count != 1)
						{
							throw new Exception("Comflic while Sync. The updated row doesn't exist.");
						}
						DataRow userInfoRow = userInfoDT.Rows[0];
						DateTime oriLastUpdatedMoney = (DateTime)userInfoRow["LastUpdatedMoney"];
						DateTime? oriLastUpdateFingerPrint = userInfoRow.Field<DateTime?>("LastUpdatedFingerPrint");

						userInfoTA.UpdateGeneric(name, typeShortName, isCafeteriaStaff, isActive, insertedDate, updatedBy, lastUpdated, username);
						if (oriLastUpdatedMoney > lastSync)
						{
							userInfoTA.UpdateMoney(amountOfMoney, lastUpdatedMoney, username);
						}

						if (oriLastUpdateFingerPrint == null || oriLastUpdateFingerPrint.Value > lastSync)
						{
							userInfoTA.UpdateFingerPrint(fingerPrintIMG, lastUpdatedFingerPrint, fingerPosition, username);
						}
					}

					//sync mealset table
					// NOTE: table MealSet in subDB only get value from db in web app.
					// so in the desktop app we only insert and update
					ElList = newDataSetEl.Descendants("MealSet").ToList();

					foreach (XElement mealSetEl in ElList)
					{
						// not null value parse
						int mealSetID = int.Parse(mealSetEl.Element("MealSetID").Value);
						string name = mealSetEl.Element("Name").Value;
						bool canEatMore = bool.Parse(mealSetEl.Element("CanEatMore").Value);
						DateTime insertedDate = DateTime.Parse(mealSetEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(mealSetEl.Element("LastUpdated").Value);

						// nullable value parse
						string updatedBy = mealSetEl.TryGetElementValue("UpdatedBy");

						if (insertedDate > lastSync)
						{
							mealSetTA.InsertWithID(mealSetID, name, canEatMore, insertedDate, updatedBy, lastUpdated);
							continue;
						}

						mealSetTA.Update(name, canEatMore, insertedDate, updatedBy, lastUpdated, mealSetID);
					}

					// sync serving time table
					// NOTE: table serving time in subDB only get value from db in web app.
					// so in the desktop app we only insert and update
					ElList = newDataSetEl.Descendants("ServingTime").ToList();

					foreach (XElement servingTimeEl in ElList)
					{
						// not null value parse
						int servingTimeID = int.Parse(servingTimeEl.Element("ServingTimeID").Value);
						string name = servingTimeEl.Element("Name").Value;
						string a = servingTimeEl.Element("StartTime").Value;
						TimeSpan startTime = XmlConvert.ToTimeSpan(servingTimeEl.Element("StartTime").Value);
						TimeSpan endTime = XmlConvert.ToTimeSpan(servingTimeEl.Element("EndTime").Value);
						DateTime insertedDate = DateTime.Parse(servingTimeEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(servingTimeEl.Element("LastUpdated").Value);

						// nullable value parse
						if (insertedDate > lastSync)
						{
							servingTimeTA.InsertWithID(servingTimeID, name, startTime, endTime, insertedDate, lastUpdated);
							continue;
						}

						servingTimeTA.Update(name, startTime, endTime, insertedDate, lastUpdated, servingTimeID);
					}

					// sync schedule table
					ElList = newDataSetEl.Descendants("Schedule").ToList();

					foreach (XElement scheduleEl in ElList)
					{
						// not null value parse
						int scheduleID = int.Parse(scheduleEl.Element("ScheduleID").Value);
						DateTime date = DateTime.Parse(scheduleEl.Element("Date").Value);
						int servingTimeID = int.Parse(scheduleEl.Element("ServingTimeID").Value);
						bool isDayOn = bool.Parse(scheduleEl.Element("IsDayOn").Value);
						DateTime insertedDate = DateTime.Parse(scheduleEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(scheduleEl.Element("LastUpdated").Value);

						// nullable value parse
						string updatedBy = scheduleEl.TryGetElementValue("UpdatedBy");

						if (insertedDate > lastSync)
						{
							scheduleTA.InsertWithID(scheduleID, servingTimeID, date, isDayOn, insertedDate, updatedBy, lastUpdated);
							continue;
						}

						scheduleTA.Update(date, servingTimeID, isDayOn, insertedDate, updatedBy, lastUpdated, scheduleID);
					}

					// sync schedule meal set detail table
					ElList = newDataSetEl.Descendants("ScheduleMealSetDetail").ToList();

					foreach (XElement scheduleMealSetDetailEl in ElList)
					{
						// not null value parse
						int scheduleMealSetDetailID = int.Parse(scheduleMealSetDetailEl.Element("ScheduleMealSetDetailID").Value);
						int mealSetID = int.Parse(scheduleMealSetDetailEl.Element("MealSetID").Value);
						int scheduleID = int.Parse(scheduleMealSetDetailEl.Element("ScheduleID").Value);
						DateTime insertedDate = DateTime.Parse(scheduleMealSetDetailEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(scheduleMealSetDetailEl.Element("LastUpdated").Value);

						if (insertedDate > lastSync)
						{
							scheduleMealSetDetailTA.InsertWithID(scheduleMealSetDetailID, mealSetID, scheduleID, insertedDate, lastUpdated);
							continue;
						}

						scheduleMealSetDetailTA.Update(mealSetID, scheduleID, insertedDate, lastUpdated, scheduleMealSetDetailID);
					}

					// sync transaction type table
					ElList = newDataSetEl.Descendants("TransactionType").ToList();

					foreach (XElement transactionTypeEl in ElList)
					{
						// not null value parse
						int transactionTypeID = int.Parse(transactionTypeEl.Element("TransactionTypeID").Value);
						string name = transactionTypeEl.Element("Name").Value;
						DateTime insertedDate = DateTime.Parse(transactionTypeEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(transactionTypeEl.Element("LastUpdated").Value);

						if (insertedDate > lastSync)
						{
							transactionTypeTA.InsertWithID(transactionTypeID, name, insertedDate, lastUpdated);
							continue;
						}

						transactionTypeTA.Update(name, insertedDate, lastUpdated, transactionTypeID);
					}

					// sync transaction history table
					ElList = newDataSetEl.Descendants("TransactionHistory").ToList();

					foreach (XElement transactionHistoryEl in ElList)
					{
						// not null value parse
						int transactionHistoryID = int.Parse(transactionHistoryEl.Element("TransactionHistoryID").Value);
						string username = transactionHistoryEl.Element("Username").Value;
						int transactionTypeID = int.Parse(transactionHistoryEl.Element("TransactionTypeID").Value);
						int value = int.Parse(transactionHistoryEl.Element("Value").Value);
						string transactionContent = transactionHistoryEl.Element("TransactionContent").Value;
						bool isAuto = bool.Parse(transactionHistoryEl.Element("IsAuto").Value);
						DateTime insertedDate = DateTime.Parse(transactionHistoryEl.Element("InsertedDate").Value);
						DateTime lastUpdated = DateTime.Parse(transactionHistoryEl.Element("LastUpdated").Value);

						// nullable value parse
						int? scheduleMealSetDetailID;
						if (transactionHistoryEl.Element("ScheduleMealSetDetailID") == null)
						{
							scheduleMealSetDetailID = null;
						}
						else
						{
							scheduleMealSetDetailID = int.Parse(transactionHistoryEl.Element("ScheduleMealSetDetailID").Value);
						}

						string updatedBy = transactionHistoryEl.TryGetElementValue("UpdatedBy");

						if (insertedDate > lastSync)
						{
							transactionHistoryTA.Insert(username, transactionTypeID, value, transactionContent
								, scheduleMealSetDetailID, isAuto, insertedDate, updatedBy, lastUpdated);
							continue;
						}

						transactionHistoryTA.Update(username, transactionTypeID, value, transactionContent, scheduleMealSetDetailID
							, isAuto, insertedDate, updatedBy, lastUpdated, transactionHistoryID);
					}

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}
			}
		}

		private static string SendXMLFile(string filename, string uri, int timeout)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

			request.KeepAlive = false;
			request.ProtocolVersion = HttpVersion.Version10;
			request.ContentType = "application/xml";
			request.Method = "POST";

			StringBuilder sb = new StringBuilder();
			using (StreamReader sr = new StreamReader(_path + filename))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
				{
					sb.AppendLine(line);
				}
				byte[] postBytes = Encoding.UTF8.GetBytes(sb.ToString());

				if (timeout < 0)
				{
					request.ReadWriteTimeout = timeout;
					request.Timeout = timeout;
				}

				request.ContentLength = postBytes.Length;

				try
				{
					Stream requestStream = request.GetRequestStream();

					requestStream.Write(postBytes, 0, postBytes.Length);
					requestStream.Close();

					using (var response = (HttpWebResponse)request.GetResponse())
					{
						return response.ToString();
					}
				}
				catch (WebException ex)
				{
					request.Abort();
					using (WebResponse response = ex.Response)
					{
						HttpWebResponse httpResponse = (HttpWebResponse)response;
						Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
						using (Stream data = response.GetResponseStream())
						using (var reader = new StreamReader(data))
						{
							string text = reader.ReadToEnd();
							Console.WriteLine(text);
						}
					}
					throw ex;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		private static string GetValueFromElement(XElement ele)
		{
			if (ele == null)
			{
				return null;
			}
			return ele.Value;
		}

		private static string TryGetElementValue(this XElement parentEl, string elementName, string defaultValue = null)
		{
			var foundEl = parentEl.Element(elementName);

			if (foundEl != null)
			{
				return foundEl.Value;
			}

			return defaultValue;
		}

		internal static void SetLastSyncAndInactiveFile(DateTime syncDate, string syncFilenames)
		{
			XDocument xDoc = XDocument.Load(_xmlConfigPath);

			List<XElement> xmlData = xDoc.Element("SyncServer").Descendants("XmlData").ToList();
			xDoc.Element("SyncServer").Attribute("LastSync").Value = syncDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

			string[] syncFileNamesArr = syncFilenames.Split('|');

			foreach (string syncFileName in syncFileNamesArr)
			{
				if (syncFileName == String.Empty)
				{
					continue;
				}

				xmlData.Where(x => x.Value == syncFileName).Single().Attribute("Active").Value = "0";
			}

			xDoc.Save(_xmlConfigPath);
		}
	}
}
