using CTMF_Desktop_App.ServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTMF_Desktop_App
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			WebServiceSoapClient soap = new WebServiceSoapClient();
			AuthSoapHeader header = new AuthSoapHeader();
			header.authString = "cc66ea17f303543d8c46207a7eaac530";
			DataTable dt = soap.GetAccount(header, "dungnmse02767");
		}
	}
}
