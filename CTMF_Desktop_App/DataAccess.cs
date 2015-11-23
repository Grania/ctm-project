using CTMF_Desktop_App.DataAccessTableAdapters;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Transactions;
using System.Configuration;
using CTMF_Desktop_App.Forms.ExtentionClass;

namespace CTMF_Desktop_App
{
	public partial class DataAccess
	{
		internal static string NewSync()
		{
			Log.ActivityLog("-----------------------New sync transaction-----------------------");
			Log.ActivityLog("Clear all table.");

			MealSetTableAdapter mealSetTA = new MealSetTableAdapter();
			ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();
			ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();
			ServingTimeTableAdapter servingTimeTA = new ServingTimeTableAdapter();
			TransactionHistoryTableAdapter transactionHistoryTA = new TransactionHistoryTableAdapter();
			TransactionTypeTableAdapter transactionTypeTA = new TransactionTypeTableAdapter();
			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			UserTypeTableAdapter userTypeTA = new UserTypeTableAdapter();

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
					transactionHistoryTA.ClearTable();
					transactionTypeTA.ClearTable();
					scheduleMealSetDetailTA.ClearTable();
					scheduleTA.ClearTable();
					servingTimeTA.ClearTable();
					mealSetTA.ClearTable();
					userInfoTA.ClearTable();
					userTypeTA.ClearTable();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw ex;
				}

				transaction.Commit();
			}

			Log.ActivityLog("Clear all complete.");
			Log.ActivityLog("Register sync to server.");

			ServiceReference.WebServiceSoapClient soapClient = new ServiceReference.WebServiceSoapClient();

			DateTime syncDate = DateTime.Now;
			string oldSyncID = XmlSync.GetSyncID();
			string newSyncID = soapClient.NewSyncData(WebServiceAuth.AuthSoapHeader(), syncDate, oldSyncID);
			XmlSync.SaveNewSync(newSyncID);

			Log.ActivityLog("Register complete.");
			Log.ActivityLog("Geting xml data.");

			string fileNameList = XmlSync.RequestXmlFileName(newSyncID, false);
			IList<string> xmlPathList = XmlSync.SaveXmlFile(fileNameList);

			Log.ActivityLog("Save xml data complete.");
			Log.ActivityLog("Start sync.");

			XmlSync.Sync(xmlPathList);

			Log.ActivityLog("Sync complete.");
			Log.ActivityLog("Update last sync date.");

			XmlSync.SetLastSync(syncDate);
			soapClient.SetLastSyncAndInactiveFile(WebServiceAuth.AuthSoapHeader(), syncDate, fileNameList,newSyncID);
			Log.ActivityLog("-----------------------Sync transaction done-----------------------");
			return newSyncID;
		}

		internal static void StartSync()
		{
			Log.ActivityLog("-----------------------Start sync transaction-----------------------");

			string syncFilenames = XmlSync.RequestSync();

			int a = 0;
		}

		internal static string PayForFood(Customer customer)
		{
			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();

			DataTable userInfo = userInfoTA.GetUserInfoWithoutFingerPrint(customer.Username);
			if (userInfo.Rows.Count != 1)
			{
				throw new Exception("Can't get User Info.");
			}

			DateTime lastUpdatedMoney = userInfo.Rows[0].Field<DateTime>("LastUpdatedMoney");
			int amountOfMoney = userInfo.Rows[0].Field<int>("AmountOfMoney");

			TransactionHistoryTableAdapter transactionHistoryTA = new TransactionHistoryTableAdapter();

			int? sumOfMoney = transactionHistoryTA.GetCurrentMoney(customer.Username, lastUpdatedMoney);

			if (sumOfMoney == null)
			{
				sumOfMoney = 0;
			}

			int curMoney = sumOfMoney.Value + amountOfMoney;

			int remainMoney = curMoney - customer.MealValue;
			if (remainMoney < 0)
			{
				if (!customer.CanDebt)
				{
					return "Tai khoan da het tien";
				}
			}

			DateTime insertedDate = DateTime.Now;
			int TransactionHistoryID = transactionHistoryTA.Insert(customer.Username, 1, (-1) * customer.MealValue, "Ăn"
				, null, true, insertedDate, CTMF_Desktop_App.Forms.MainForm.username, insertedDate);

			XmlSync.SaveTransactionHistoryXml(TransactionHistoryID, customer.Username, 1, (-1) * customer.MealValue, "Ăn"
				, null, true, insertedDate, CTMF_Desktop_App.Forms.MainForm.username, insertedDate);

			return "Con lai:" + remainMoney;
		}
	}
}

namespace CTMF_Desktop_App.DataAccessTableAdapters
{
	public partial class UserTypeTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class UserInfoTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class MealSetTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class ServingTimeTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class ScheduleTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class ScheduleMealSetDetailTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class TransactionTypeTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class TransactionHistoryTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}

	public partial class SyncDateTableAdapter
	{
		public void AttachTransaction(System.Data.SqlClient.SqlTransaction t)
		{
			this.Adapter.InsertCommand.Transaction = t;
			this.Adapter.UpdateCommand.Transaction = t;
			this.Adapter.DeleteCommand.Transaction = t;
			foreach (System.Data.SqlClient.SqlCommand cmd
					 in this.CommandCollection)
			{
				cmd.Transaction = t;
			}
		}
	}
}
