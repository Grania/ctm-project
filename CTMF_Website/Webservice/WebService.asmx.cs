using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace CTMF_Website.Webservice
{
	/// <summary>
	/// Summary description for WebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class WebService : System.Web.Services.WebService
	{
		public AuthSoapHeader soapHeader;

		public WebService()
		{
		}

		public WebService(string authString)
		{
			soapHeader.authString = authString;
		}

		public WebService(AuthSoapHeader soapHeader)
		{
			this.soapHeader = soapHeader;
		}

		private bool authHeader()
		{
			if (soapHeader == null)
			{
				return false;
			}
			if (soapHeader.authString == "cc66ea17f303543d8c46207a7eaac530")
			{
				return true;
			}
			return false;
		}

		[WebMethod, SoapHeader("soapHeader")]
		public DataTable GetAccount(string username)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			AccountTableAdapter adapter = new AccountTableAdapter();
			DataTable dt = null;
			try
			{
				dt = adapter.GetDataByUsername(username);
				SqlCommand sc = adapter.Adapter.SelectCommand;
				return dt;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		[WebMethod, SoapHeader("soapHeader")]
		public bool getUpdatedFingerPrint(string username, byte[] image, int? fingerPosition
			, DateTime lastUpdatedFingerPrint,string ignoreSyncID)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			try
			{
				DataTable userInfoDT = userInfoTA.GetDataByUsername(username);
				if (userInfoDT.Rows.Count != 1)
				{
					throw new Exception("Error while get user info by username = " + username);
				}

				DataRow row = userInfoDT.Rows[0];
				row.BeginEdit();
				row["FingerPrintIMG"] = image;
				row["FingerPosition"] = fingerPosition;
				row["LastUpdatedFingerPrint"] = lastUpdatedFingerPrint;
				row.AcceptChanges();

				userInfoTA.Update(row.Field<string>("Username"), row.Field<string>("Name"), row.Field<string>("TypeShortName")
					, row.Field<int>("AmountOfMoney"), row.Field<DateTime>("LastUpdatedMoney"), image, lastUpdatedFingerPrint
					, fingerPosition, row.Field<bool>("IsCafeteriaStaff"), row.Field<bool>("IsActive"), row.Field<DateTime>("InsertedDate")
					, row.Field<string>("UpdatedBy"), row.Field<DateTime>("LastUpdated"), username);
				XmlSync.SaveUserInfoXml(row.Field<string>("Username"), row.Field<string>("Name"), row.Field<string>("TypeShortName")
					, row.Field<int>("AmountOfMoney"), row.Field<DateTime>("LastUpdatedMoney"), image, lastUpdatedFingerPrint
					, fingerPosition, row.Field<bool>("IsCafeteriaStaff"), row.Field<bool>("IsActive"), row.Field<DateTime>("InsertedDate")
					, row.Field<string>("UpdatedBy"), row.Field<DateTime>("LastUpdated"), ignoreSyncID);
				return true;
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return false;
			}
		}

		[WebMethod, SoapHeader("soapHeader")]
		public string NewSyncData(DateTime toDate, string oldSyncID)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			return XmlSync.GetNewSync(toDate, oldSyncID);
		}

		[WebMethod, SoapHeader("soapHeader")]
		public bool DeleteSync(string syncID)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			return XmlSync.DeleteSync(syncID);
		}

		[WebMethod, SoapHeader("soapHeader")]
		public string RequestXmlFileName(string syncID, bool isExcludeCurrent)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			return XmlSync.RequestXmlFileName(syncID, isExcludeCurrent);
		}

		[WebMethod, SoapHeader("soapHeader")]
		public void SetLastSyncAndInactiveFile(DateTime syncDate, string fileNameList, string syncID)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			XmlSync.SetLastSyncAndInactiveFile(syncDate, fileNameList, syncID);
		}

		[WebMethod, SoapHeader("soapHeader")]
		public string RequestSync(string syncID, string filenames)
		{
			if (!authHeader())
			{
				throw new Exception(StringResources.E00001);
			}

			return XmlSync.RequestSync(syncID, filenames);
		}
	}
}
