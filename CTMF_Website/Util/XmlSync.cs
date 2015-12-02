using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CTMF_Website.DataAccessTableAdapters;
using System.Data.SqlClient;

namespace CTMF_Website.Util
{
	public static class XmlSync
	{
		private static string _path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "xml\\";
		private static string _configFilePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "xml\\config.xml";

		internal static string GetNewSync(DateTime toDate, string oldSyncID)
		{
			XDocument xDoc = XDocument.Load(_configFilePath);
			XElement clientsEl = xDoc.Element("Clients");

			if (oldSyncID != null && oldSyncID != String.Empty)
			{
				try
				{
					clientsEl.Descendants("Client").Where(c => c.Attribute("SyncID") != null
						&& c.Attribute("SyncID").Value == oldSyncID).Single().Remove();
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}

			int resignedClient = 0;
			if (clientsEl.Attribute("resignedClient") != null)
			{
				int.TryParse(clientsEl.Attribute("resignedClient").Value, out resignedClient);
			}
			else
			{
				clientsEl.Add(new XAttribute("resignedClient", 0));
			}

			string syncID = toDate.ToString("yyyyMMddHHmmssfff") + resignedClient;

			DataSet ds = DataAccess.GetSyncData(toDate);

			StringBuilder sbXmlFileName = new StringBuilder();
			sbXmlFileName.Append("Client");
			sbXmlFileName.Append(resignedClient.ToString());
			sbXmlFileName.Append(toDate.ToString("-yyyyMMddHHmmssfff"));
			sbXmlFileName.Append(".xml");
			string xmlFileName = sbXmlFileName.ToString();

			XDocument xDocDS = GetXDocumentFromDataSet(ds);
			xDocDS.Save(_path + xmlFileName);

			XElement clientEl = new XElement("Client");
			clientEl.Add(new XAttribute("Name", "Client" + resignedClient));
			clientEl.Add(new XAttribute("SyncID", syncID));
			clientEl.Add(new XAttribute("LastSync", new DateTime().ToString("yyyy-MM-dd HH:mm:ss.fff")));
			XElement xmlDataEl = new XElement("XmlData");
			xmlDataEl.Add(new XAttribute("Active", 1));
			xmlDataEl.Add(new XAttribute("Current", 0));
			xmlDataEl.Value = xmlFileName;
			clientEl.Add(xmlDataEl);

			StringBuilder sbCurXmlFileName = new StringBuilder();
			sbCurXmlFileName.Append("Client");
			sbCurXmlFileName.Append(resignedClient.ToString());
			sbCurXmlFileName.Append(DateTime.Now.ToString("-yyyyMMddHHmmssfff"));
			sbCurXmlFileName.Append(".xml");
			string curXmlFileName = sbCurXmlFileName.ToString();

			XDocument curXml = new XDocument();
			curXml.Add(new XElement("NewDataSet"));
			curXml.Save(_path + curXmlFileName);

			XElement curXmlDataEl = new XElement("XmlData");
			curXmlDataEl.Add(new XAttribute("Active", 1));
			curXmlDataEl.Add(new XAttribute("Current", 1));
			curXmlDataEl.Value = curXmlFileName;
			clientEl.Add(curXmlDataEl);

			clientsEl.Add(clientEl);

			resignedClient++;
			clientsEl.Attribute("resignedClient").Value = resignedClient.ToString();
			xDoc.Save(_configFilePath);

			return syncID;
		}

		internal static void SetLastSyncAndInactiveFile(DateTime syncDate, string fileNameList, string syncID)
		{
			XDocument xDoc = XDocument.Load(_configFilePath);

			try
			{
				XElement clientEl = xDoc.Element("Clients").Descendants("Client").Where(c => c.Attribute("SyncID") != null
					&& c.Attribute("SyncID").Value == syncID).Single();

				XAttribute lastSyncAttr = clientEl.Attribute("LastSync");

				if (lastSyncAttr == null)
				{
					clientEl.Add(new XAttribute("LastSync", syncDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
				}
				else
				{
					lastSyncAttr.Value = syncDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
				}

				string[] xmlFileNameList = fileNameList.Split('|');
				foreach (string xmlFileName in xmlFileNameList)
				{
					if (xmlFileName.Trim() == String.Empty)
					{
						continue;
					}

					clientEl.Descendants("XmlData").Where(x => x.Value == xmlFileName).Single().Attribute("Active").Value = "0";
				}

				xDoc.Save(_configFilePath);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static XDocument GetXDocumentFromDataSet(DataSet dataSet)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.None })
				{
					dataSet.WriteXml(xmlTextWriter);
					memoryStream.Position = 0;
					var xmlReader = XmlReader.Create(memoryStream);
					xmlReader.MoveToContent();
					return XDocument.Load(xmlReader);
				}
			}
		}

		internal static string RequestXmlFileName(string syncID, bool isExcludeCurrent)
		{
			XDocument xDoc = XDocument.Load(_configFilePath);

			try
			{
				XElement clientsEl = xDoc.Element("Clients");

				XElement clientEl = clientsEl.Descendants("Client").Where(c => c.Attribute("SyncID") != null
					&& c.Attribute("SyncID").Value == syncID).Single();

				List<XElement> listXmlData = clientEl.Descendants("XmlData").ToList();

				if (!isExcludeCurrent)
				{
					clientEl.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1")
						.Single().Attribute("Current").Value = "0";

					StringBuilder sbXmlFileName = new StringBuilder();
					sbXmlFileName.Append(clientEl.Attribute("Name").Value);
					sbXmlFileName.Append(DateTime.Now.ToString("-yyyyMMddHHmmssfff"));
					sbXmlFileName.Append(".xml");
					string xmlFileName = sbXmlFileName.ToString();

					XDocument newDataSetDoc = new XDocument();
					newDataSetDoc.Add(new XElement("NewDataSet"));
					newDataSetDoc.Save(_path + xmlFileName);

					XElement xmlDataEl = new XElement("XmlData");
					xmlDataEl.Add(new XAttribute("Active", 1));
					xmlDataEl.Add(new XAttribute("Current", 1));
					xmlDataEl.Value = xmlFileName;
					clientEl.Add(xmlDataEl);
				}

				listXmlData = listXmlData.Where(c => c.Attribute("Active").Value == "1"
						&& c.Attribute("Current").Value == "0").ToList();

				xDoc.Save(_configFilePath);

				StringBuilder sbFileNameList = new StringBuilder();
				foreach (XElement el in listXmlData)
				{
					sbFileNameList.Append(el.Value);
					sbFileNameList.Append("|");
				}

				return sbFileNameList.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static bool DeleteSync(string syncID)
		{
			XDocument xDoc = XDocument.Load(_configFilePath);

			try
			{
				xDoc.Element("Clients").Descendants("Client").Where(c => c.Attribute("SyncID") != null
					&& c.Attribute("SyncID").Value == syncID).Single().Remove();
			}
			catch (Exception)
			{
				return false;
			}

			xDoc.Save(_configFilePath);
			return true;
		}

		//return list xml file to sync from server
		internal static string RequestSync(string syncID, string filenames)
		{
			IList<string> xmlFilePathList = new List<string>();
			string xmlFilenames = RequestXmlFileName(syncID, false);

			string[] xmlFilename = filenames.Split('|');

			foreach (string filename in xmlFilename)
			{
				if (filename.Trim() == String.Empty)
				{
					continue;
				}

				xmlFilePathList.Add(_path + filename);
			}

			sync(xmlFilePathList, syncID);

			return xmlFilenames;
		}

		internal static void sync(IList<string> xmlFilePathList, string ignoreSyncID)
		{
			foreach (string xmlFilePath in xmlFilePathList)
			{
				sync(xmlFilePath, ignoreSyncID);
			}
		}

		private static void sync(string xmlFilePath, string ignoreSyncID)
		{
			XDocument xDoc;
			XElement newDataSetEl;
			DateTime lastSync;


			xDoc = XDocument.Load(xmlFilePath);
			newDataSetEl = xDoc.Element("NewDataSet");
			lastSync = GetLastUpdatedClient(ignoreSyncID);

			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			TransactionHistoryTableAdapter transactionHistoryTA = new TransactionHistoryTableAdapter();

			userInfoTA.Connection.Open();
			transactionHistoryTA.Connection = userInfoTA.Connection;

			using (SqlTransaction transaction = userInfoTA.Connection.BeginTransaction())
			{
				userInfoTA.AttachTransaction(transaction);
				transactionHistoryTA.AttachTransaction(transaction);

				try
				{
					IList<XElement> ElList = newDataSetEl.Descendants("UserInfo").ToList();

					foreach (XElement userInfoEl in ElList)
					{
						// not null value parse
						string username = userInfoEl.Element("Username").Value;

						// nullable value parse
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

						DataTable userInfoDT = new UserInfoTableAdapter().GetDataByUsername(username);
						if (userInfoDT.Rows.Count != 1)
						{
							throw new Exception("Comflic while Sync. The updated row doesn't exist.");
						}

						DataRow userInfoRow = userInfoDT.Rows[0];
						DateTime? oriLastUpdateFingerPrint = userInfoRow.Field<DateTime?>("LastUpdatedFingerPrint");
						if (oriLastUpdateFingerPrint == null || (lastUpdatedFingerPrint != null
							&& lastUpdatedFingerPrint > oriLastUpdateFingerPrint))
						{
							//userInfoTA.Update(userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
							//	, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerPrintIMG
							//	, lastUpdatedFingerPrint, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff")
							//	, userInfoRow.Field<bool>("IsActive"), userInfoRow.Field<DateTime>("InsertedDate"), userInfoRow.Field<string>("UpdatedBy")
							//	, userInfoRow.Field<DateTime>("LastUpdated"), username);

							userInfoTA.Update(userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
								, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerPrintIMG
								, lastUpdatedFingerPrint, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff")
								, true, userInfoRow.Field<DateTime>("InsertedDate"), userInfoRow.Field<string>("UpdatedBy")
								, userInfoRow.Field<DateTime>("LastUpdated"), username);

							//SaveUserInfoXml(userInfoRow.Field<string>("Username"), userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
							//	, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerPrintIMG
							//	, lastUpdatedFingerPrint, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff")
							//	, userInfoRow.Field<bool>("IsActive"), userInfoRow.Field<DateTime>("InsertedDate"), userInfoRow.Field<string>("UpdatedBy")
							//	, userInfoRow.Field<DateTime>("LastUpdated"), ignoreSyncID);

							SaveUserInfoXml(userInfoRow.Field<string>("Username"), userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
								, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerPrintIMG
								, lastUpdatedFingerPrint, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff")
								,true, userInfoRow.Field<DateTime>("InsertedDate"), userInfoRow.Field<string>("UpdatedBy")
								, userInfoRow.Field<DateTime>("LastUpdated"), ignoreSyncID);
						}
					}

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
							transactionHistoryTA.Insert(username, transactionHistoryID, value, transactionContent, scheduleMealSetDetailID,
								isAuto, insertedDate, updatedBy, lastUpdated);

							SaveTransactionHistoryXml(transactionHistoryID, username, transactionTypeID, value, transactionContent
								, scheduleMealSetDetailID, isAuto, insertedDate, updatedBy, lastUpdated, ignoreSyncID);
						}
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

		#region Save data to xml

		internal static void SaveUserTypeXml(string typeShortName, string typeName, int mealValue, int? moreMealValue,
			string description, bool canDebt, bool canEatMore, DateTime insertedDate, string updatedBy
			, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients").Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement userType = new XElement("UserType");

				// not null value save
				userType.Add(new XElement("TypeShortName", typeShortName));
				userType.Add(new XElement("TypeName", typeName));
				userType.Add(new XElement("MealValue", mealValue));
				userType.Add(new XElement("CanDebt", canDebt));
				userType.Add(new XElement("CanEatMore", canEatMore));
				userType.Add(new XElement("InsertedDate", insertedDate));
				userType.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (moreMealValue != null)
				{
					userType.Add(new XElement("MoreMealValue", moreMealValue));
				}
				if (description != null)
				{
					userType.Add(new XElement("Description", description));
				}
				if (updatedBy != null)
				{
					userType.Add(new XElement("UpdatedBy", updatedBy));
				}

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("UserType").Where(d => d.Element("TypeShortName").Value == typeShortName).Remove();

					dataSetEl.Add(userType);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveUserInfoXml(string username, string name, string typeShortName, int amountOfMoney
			, DateTime lastUpdatedMoney, byte[] fingerPrintIMG, DateTime? lastUpdatedFingerPrint, int? fingerPosition
			, bool isCafeteriaStaff, bool isActive, DateTime insertedDate, string updatedBy, DateTime lastUpdated
			, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement userInfo = new XElement("UserInfo");

				// not null value save
				userInfo.Add(new XElement("Username", username));
				userInfo.Add(new XElement("AmountOfMoney", amountOfMoney));
				userInfo.Add(new XElement("LastUpdatedMoney", lastUpdatedMoney));
				userInfo.Add(new XElement("ICafeteriaStaff", isCafeteriaStaff));
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
					userInfo.Add(new XElement("FingerPrintIMG", fingerPrintIMG));
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

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("UserInfo").Where(d => d.Element("Username").Value == typeShortName).Remove();

					dataSetEl.Add(userInfo);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveMealSetXml(int mealSetID, string name, int usedTime, bool canEatMore, DateTime insertedDate
			, string updatedBy, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement mealSet = new XElement("MealSet");

				// not null value save
				mealSet.Add(new XElement("MealSetID", mealSetID));
				mealSet.Add(new XElement("Name", name));
				mealSet.Add(new XElement("UsedTime", usedTime));
				mealSet.Add(new XElement("CanEatMore", canEatMore));
				mealSet.Add(new XElement("InsertedDate", insertedDate));
				mealSet.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (updatedBy != null)
				{
					mealSet.Add(new XElement("UpdatedBy", updatedBy));
				}

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("MealSet").Where(d => d.Element("MealSetID").Value == mealSetID.ToString()).Remove();

					dataSetEl.Add(mealSet);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveServingTimeXml(int servingTimeID, string name, TimeSpan startTime, TimeSpan? endTime
			, DateTime insertedDate, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement servingTime = new XElement("MealSet");

				// not null value save
				servingTime.Add(new XElement("ServingTimeID", servingTimeID));
				servingTime.Add(new XElement("Name", name));
				servingTime.Add(new XElement("StartTime", startTime));
				servingTime.Add(new XElement("InsertedDate", insertedDate));
				servingTime.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (endTime != null)
				{
					servingTime.Add(new XElement("EndTime", endTime));
				}

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("ServingTime").Where(d => d.Element("ServingTimeID").Value == servingTimeID.ToString()).Remove();

					dataSetEl.Add(servingTime);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveScheduleXml(int scheduleID, DateTime date, int servingTimeID, bool isDayOn
			, DateTime insertedDate, string updatedBy, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement schedule = new XElement("Schedule");

				// not null value save
				schedule.Add(new XElement("ScheduleID", scheduleID));
				schedule.Add(new XElement("Date", date));
				schedule.Add(new XElement("ServingTimeID", servingTimeID));
				schedule.Add(new XElement("InsertedDate", insertedDate));
				schedule.Add(new XElement("LastUpdated", lastUpdated));

				// nullable value save
				if (updatedBy != null)
				{
					schedule.Add(new XElement("UpdatedBy", updatedBy));
				}

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("Schedule").Where(d => d.Element("ScheduleID").Value == scheduleID.ToString()).Remove();

					dataSetEl.Add(schedule);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveScheduleMealSetDetailXml(int scheduleMealSetDetailID, int mealSetID, int scheduleID
			, string name, DateTime insertedDate, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("ScheduleMealSetDetail").Where(d => d.Element("ScheduleMealSetDetailID").Value == scheduleMealSetDetailID.ToString()).Remove();

					XElement scheduleMealSetDetail = new XElement("ScheduleMealSetDetail");

					// not null value save
					scheduleMealSetDetail.Add(new XElement("ScheduleMealSetDetailID", scheduleMealSetDetailID));
					scheduleMealSetDetail.Add(new XElement("MealSetID", mealSetID));
					scheduleMealSetDetail.Add(new XElement("ScheduleID", scheduleID));
					scheduleMealSetDetail.Add(new XElement("Name", name));
					scheduleMealSetDetail.Add(new XElement("InsertedDate", insertedDate));
					scheduleMealSetDetail.Add(new XElement("LastUpdated", lastUpdated));

					dataSetEl.Add(scheduleMealSetDetail);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveTransactionTypeXml(int transactionTypeID, string name, DateTime insertedDate, DateTime lastUpdated
			, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

				XElement transactionType = new XElement("TransactionType");

				// not null value save
				transactionType.Add(new XElement("TransactionTypeID", transactionTypeID));
				transactionType.Add(new XElement("Name", name));
				transactionType.Add(new XElement("InsertedDate", insertedDate));
				transactionType.Add(new XElement("LastUpdated", lastUpdated));

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("TransactionType").Where(d => d.Element("TransactionTypeID").Value == transactionTypeID.ToString()).Remove();

					dataSetEl.Add(transactionType);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal static void SaveTransactionHistoryXml(int transactionHistoryID, string username, int transactionTypeID
			, int value, string transactionContent, int? scheduleMealSetDetailID, bool isAuto, DateTime insertedDate
			, string updatedBy, DateTime lastUpdated, string ignoreSyncID)
		{
			try
			{
				XDocument xDoc = XDocument.Load(_configFilePath);

				List<XElement> xmlDataEls = xDoc.Element("Clients")
					.Descendants("Client").Where(c => c.Attribute("SyncID").Value != ignoreSyncID)
					.Descendants("XmlData").Where(x => x.Attribute("Current").Value == "1").ToList();

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
				if (scheduleMealSetDetailID == null)
				{
					transactionHistory.Add(new XElement("ScheduleMealSetDetailID", scheduleMealSetDetailID));
				}
				if (updatedBy == null)
				{
					transactionHistory.Add(new XElement("UpdatedBy", updatedBy));
				}

				foreach (XElement xmlDataEl in xmlDataEls)
				{
					XDocument dataSetXDoc = XDocument.Load(_path + xmlDataEl.Value);
					XElement dataSetEl = dataSetXDoc.Element("NewDataSet");

					dataSetEl.Descendants("TransactionHistory").Where(d => d.Element("TransactionHistoryID").Value == transactionHistoryID.ToString()).Remove();

					dataSetEl.Add(transactionHistory);

					dataSetXDoc.Save(_path + xmlDataEl.Value);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		private static DateTime GetLastUpdatedClient(string syncID)
		{
			XDocument xDoc = XDocument.Load(_configFilePath);

			DateTime lastSync;
			try
			{
				lastSync = DateTime.Parse(xDoc.Element("Clients").Descendants("Client")
					.Where(c => c.Attribute("SyncID").Value == syncID).Single()
					.Attribute("LastSync").Value);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return lastSync;
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
	}
}