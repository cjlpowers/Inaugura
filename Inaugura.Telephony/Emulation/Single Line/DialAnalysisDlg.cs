using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Inaugura.Telephony.Emulation
{
    public partial class DialAnalysisDlg : Form
    {
        #region Variables
        private Inaugura.Telephony.DialAnalysis mResult = DialAnalysis.CallConnected;
        #endregion

        #region Properties
        public DialAnalysis Result
        {
            get
            {
                return this.mResult;
            }
        }
        #endregion

        public DialAnalysisDlg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.mResult = (DialAnalysis)this.comboBox1.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }

        private void DialAnalysisDlg_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.Add(DialAnalysis.Busy);
            this.comboBox1.Items.Add(DialAnalysis.CallConnected);
            this.comboBox1.Items.Add(DialAnalysis.Error);
            this.comboBox1.Items.Add(DialAnalysis.FaxTone);
            this.comboBox1.Items.Add(DialAnalysis.NoAnswer);
            this.comboBox1.Items.Add(DialAnalysis.NoDialTone);
            this.comboBox1.Items.Add(DialAnalysis.NoRingBack);
            this.comboBox1.Items.Add(DialAnalysis.OperatorIntercept);
        }
    }
}