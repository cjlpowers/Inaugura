using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Inaugura
{
	/// <summary>
	/// Summary description for FilterListEditor.
	/// </summary>
	internal class DetailsEditor: UITypeEditor
	{
		public DetailsEditor()
		{				
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,	object value)		
		{
			InitDialog((Details)value);
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private void InitDialog(Details details)
		{
			DetailsDlg dlg = new DetailsDlg(details);
			dlg.ShowDialog();
		}
	}
}
