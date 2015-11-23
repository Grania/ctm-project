using System;
using System.Windows.Forms;
using System.Data;
using CTMF_Desktop_App.Forms;
using CTMF_Desktop_App.Util;
using CTMF_Desktop_App.ServiceReference;
namespace CTMF_Desktop_App
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();

			lblAlert.Text = string.Empty;

			//for testing
			txtUsername.Text = "dungnmse02767";
			txtPassword.Text = "123456";
		}

		private void txt_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				//redirect to click Login button
				btnLogin_Click(sender, e);
			}
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			//get username, password
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text.Trim();

			//validate
			if (username == string.Empty || password == string.Empty)
			{
				lblAlert.Text = StringResource.A00001;
				return;
			}

			WebServiceSoapClient soapClient = new WebServiceSoapClient();

			//ACCOUNTTableAdapter accAdapter = new ACCOUNTTableAdapter();
			DataTable dt = null;
			try
			{
				dt = soapClient.GetAccount(WebServiceAuth.AuthSoapHeader(), username);
			}
			catch (Exception ex)
			{
				MessageBox.Show(StringResource.A00007);
				Log.ErrorLog(ex.Message);
				return;
			}

			if (dt.Rows.Count != 1)
			{
				lblAlert.Text = StringResource.A00002;
				return;
			}
			
			//check password
			if ((string)dt.Rows[0]["PASSWORD"] != password)
			{
				lblAlert.Text = StringResource.A00003;
				return;
			}

			this.Dispose(false);
			MainForm mf = new MainForm();
			mf.Show();
		}
	}
}
