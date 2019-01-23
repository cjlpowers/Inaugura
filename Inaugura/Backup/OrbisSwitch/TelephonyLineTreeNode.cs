#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using Inaugura.Telephony;


namespace OrbisSwitch
{

	class TelephonyLineTreeNode: StatusTreeNode
	{
		public TelephonyLineTreeNode(TelephonyLine line) : base(line)
		{			
			this.Tag = line;
			this.ImageIndex = 2;
			this.SelectedImageIndex = 2;			
		}
	}

}