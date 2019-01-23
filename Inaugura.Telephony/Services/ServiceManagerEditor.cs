using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

using Inaugura.Windows.Forms;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Summary description for FilterListEditor.
	/// </summary>
	public class ServiceManagerEditor : UITypeEditor
	{
		public ServiceManagerEditor()
		{				
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,	object value)		
		{
			InitDialog((ServiceManager)value);

			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private void InitDialog(ServiceManager obj)
		{
			ConfigItemDlg dlg = new ConfigItemDlg(obj);			
			dlg.ShowDialog();
		}
	}
}
