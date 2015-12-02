using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace CTMF_Website
{
	public partial class DataAccess
	{
		internal static DataSet GetSyncData(DateTime toDate)
		{
			MealSetTableAdapter mealSetTA = new MealSetTableAdapter();
			ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();
			ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();
			ServingTimeTableAdapter servingTimeTA = new ServingTimeTableAdapter();
			TransactionHistoryTableAdapter transactionHistoryTA = new TransactionHistoryTableAdapter();
			TransactionTypeTableAdapter transactionTypeTA = new TransactionTypeTableAdapter();
			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			UserTypeTableAdapter userTypeTA = new UserTypeTableAdapter();

			DateTime minDate = (DateTime)SqlDateTime.MinValue;

			DataSet ds = new DataSet();
			ds.Tables.Add(userTypeTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(userInfoTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(mealSetTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(servingTimeTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(scheduleTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(scheduleMealSetDetailTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(transactionTypeTA.GetDataByDate(minDate, toDate));
			ds.Tables.Add(transactionHistoryTA.GetDataByDate(minDate, toDate));

			return ds;
		}
	}
}

namespace CTMF_Website.DataAccessTableAdapters
{
	public partial class AccountTableAdapter
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
}
