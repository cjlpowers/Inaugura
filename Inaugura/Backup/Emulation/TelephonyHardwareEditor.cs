using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Summary description for FilterListEditor.
	/// </summary>
	public class TelephonyHardwareEditor : UITypeEditor
	{
		private TelephonyHardware mHardware = null;
		public TelephonyHardwareEditor()
		{				
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,	object value)		
		{
			this.mHardware = (TelephonyHardware)value;
			InitDialog(this.mHardware);
			return this.mHardware;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private void InitDialog(TelephonyHardware hardware)
		{
			HardwareSelectorDlg dlg = new HardwareSelectorDlg(this.mHardware);
			dlg.ShowDialog();

			this.mHardware = dlg.Hardware;
		}
	}
}
