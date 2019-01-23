using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Summary description for FilterListEditor.
	/// </summary>
	public class ServiceCollectionEditor : UITypeEditor
	{
		public ServiceCollectionEditor()
		{				
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,	object value)		
		{
			InitDialog((ServiceCollection)value);

			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private void InitDialog(ServiceCollection list)
		{
			ServiceCollectionDlg dlg = new ServiceCollectionDlg(list);
			dlg.ShowDialog();
		}
	}
}
