namespace CTMF_Desktop_App.Forms.Modal
{
	partial class UpdateServingTime
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbxServingTime = new System.Windows.Forms.ComboBox();
			this.btnCheckScheduleMealSet = new System.Windows.Forms.Button();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ngày :";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(92, 10);
			this.txtDate.Name = "txtDate";
			this.txtDate.ReadOnly = true;
			this.txtDate.Size = new System.Drawing.Size(178, 20);
			this.txtDate.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(47, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Bữa ăn :";
			// 
			// cbxServingTime
			// 
			this.cbxServingTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxServingTime.FormattingEnabled = true;
			this.cbxServingTime.Location = new System.Drawing.Point(92, 56);
			this.cbxServingTime.Name = "cbxServingTime";
			this.cbxServingTime.Size = new System.Drawing.Size(121, 21);
			this.cbxServingTime.TabIndex = 3;
			// 
			// btnCheckScheduleMealSet
			// 
			this.btnCheckScheduleMealSet.Location = new System.Drawing.Point(19, 98);
			this.btnCheckScheduleMealSet.Name = "btnCheckScheduleMealSet";
			this.btnCheckScheduleMealSet.Size = new System.Drawing.Size(140, 23);
			this.btnCheckScheduleMealSet.TabIndex = 4;
			this.btnCheckScheduleMealSet.Text = "Xem món ăn trong bữa";
			this.btnCheckScheduleMealSet.UseVisualStyleBackColor = true;
			this.btnCheckScheduleMealSet.Click += new System.EventHandler(this.btnCheckScheduleMealSet_Click);
			// 
			// dataGridView
			// 
			this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Location = new System.Drawing.Point(19, 128);
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.Size = new System.Drawing.Size(251, 192);
			this.dataGridView.TabIndex = 5;
			// 
			// btnOk
			// 
			this.btnOk.Enabled = false;
			this.btnOk.Location = new System.Drawing.Point(195, 326);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Đồng ý";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// UpdateServingTime
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 361);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.btnCheckScheduleMealSet);
			this.Controls.Add(this.cbxServingTime);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtDate);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "UpdateServingTime";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "UpdateServingTime";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbxServingTime;
		private System.Windows.Forms.Button btnCheckScheduleMealSet;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.Button btnOk;
	}
}