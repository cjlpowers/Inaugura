namespace Inaugura
{
	internal partial class DetailDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.mTxtValue = new System.Windows.Forms.TextBox();
			this.mTxtKey = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.mBtnCancel = new System.Windows.Forms.Button();
			this.mBtnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
// 
// mTxtValue
// 
			this.mTxtValue.Location = new System.Drawing.Point(90, 40);
			this.mTxtValue.Name = "mTxtValue";
			this.mTxtValue.Size = new System.Drawing.Size(271, 20);
			this.mTxtValue.TabIndex = 1;
// 
// mTxtKey
// 
			this.mTxtKey.Location = new System.Drawing.Point(90, 13);
			this.mTxtKey.Name = "mTxtKey";
			this.mTxtKey.Size = new System.Drawing.Size(271, 20);
			this.mTxtKey.TabIndex = 2;
// 
// label1
// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(56, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 14);
			this.label1.TabIndex = 3;
			this.label1.Text = "Key:";
// 
// label2
// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(47, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 14);
			this.label2.TabIndex = 4;
			this.label2.Text = "Value:";
// 
// mBtnCancel
// 
			this.mBtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.mBtnCancel.Location = new System.Drawing.Point(204, 68);
			this.mBtnCancel.Name = "mBtnCancel";
			this.mBtnCancel.TabIndex = 5;
			this.mBtnCancel.Text = "Cancel";
			this.mBtnCancel.Click += new System.EventHandler(this.mBtnCancel_Click);
// 
// mBtnOk
// 
			this.mBtnOk.Location = new System.Drawing.Point(286, 68);
			this.mBtnOk.Name = "mBtnOk";
			this.mBtnOk.TabIndex = 6;
			this.mBtnOk.Text = "Ok";
			this.mBtnOk.Click += new System.EventHandler(this.mBtnOk_Click);
// 
// DetailDlg
// 
			this.AcceptButton = this.mBtnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.mBtnCancel;
			this.ClientSize = new System.Drawing.Size(374, 100);
			this.Controls.Add(this.mBtnOk);
			this.Controls.Add(this.mBtnCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.mTxtKey);
			this.Controls.Add(this.mTxtValue);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DetailDlg";
			this.Text = "Detail";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox mTxtValue;
		private System.Windows.Forms.TextBox mTxtKey;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button mBtnCancel;
		private System.Windows.Forms.Button mBtnOk;
	}
}