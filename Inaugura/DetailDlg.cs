#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Inaugura
{
	internal partial class DetailDlg : Form
	{

		public string Key
		{
			get
			{
				return this.mTxtKey.Text;
			}
			set
			{
				this.mTxtKey.Text = value;
			}
		}

		public string Value
		{
			get
			{
				return this.mTxtValue.Text;
			}
			set
			{
				this.mTxtValue.Text = value;
			}
		}

		public DetailDlg(string key, string value)
		{
			InitializeComponent();

			this.Key = key;
			this.Value = value;
		}

		private void mBtnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void mBtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}