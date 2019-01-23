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
	public class TelephonyManagerEditor : UITypeEditor
	{
		public TelephonyManagerEditor()
		{				
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,	object value)		
		{
			InitDialog((TelephonyManager)value);

			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private void InitDialog(TelephonyManager obj)
		{
			TelephonyManagerEditorDlg dlg = new TelephonyManagerEditorDlg(obj);
			dlg.ShowDialog();
		}
	}
}
